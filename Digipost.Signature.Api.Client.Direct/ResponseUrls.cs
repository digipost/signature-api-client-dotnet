using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class ResponseUrls
    {
        private readonly Uri _statusBaseUrl;

        public ResponseUrls(Uri redirectUrl, Uri statusBaseUrl)
        {
            _statusBaseUrl = statusBaseUrl;
            Redirect = new RedirectReference(redirectUrl);
        }

        public RedirectReference Redirect { get; set; }

        public Uri StatusBaseUrl
        {
            get
            {
                if (_statusBaseUrl == null)
                {
                    throw new InvalidOperationException("The status base URL is not available. This is most likely because status for this job is retrieved by polling. " +
                                                        "Use DirectClient.GetStatusChange() to check for updated statuses for this job.");
                }
                return _statusBaseUrl;
            }
        }

        public override string ToString()
        {
            return $"Redirect: {Redirect}, StatusBaseUrl: {StatusBaseUrl}";
        }

        /// <summary>
        ///     A <see cref="StatusReference" /> is constructed from the url acquired from
        ///     <see cref="JobResponse.ResponseUrls" />, and a token provided as a
        /// </summary>
        /// <param name="statusQueryToken">
        ///     The <see cref="StatusReference.StatusQueryTokenParamName">query parameter</see> which is needed to create a
        ///     reference. This token is appended to the <see cref="ExitUrls">exit Url</see>, which the signer is redirected to
        ///     when the signing ceremony is completed/aborted/failed. The token needs to be consumed by the system the user is
        ///     redirected to, and consequently provided to the constructor and passed to <see cref="DirectClient.GetStatus" />.
        /// </param>
        /// <returns></returns>
        public StatusReference Status(string statusQueryToken)
        {
            return new StatusReference(StatusBaseUrl, statusQueryToken);
        }
    }
}