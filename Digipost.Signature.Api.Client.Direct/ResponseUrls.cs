using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class ResponseUrls
    {
        public readonly Uri StatusBaseUrl;

        public ResponseUrls(Uri redirectUrl, Uri statusBaseUrl)
        {
            StatusBaseUrl = statusBaseUrl;
            Redirect = new RedirectReference(redirectUrl);
        }

        public RedirectReference Redirect { get; set; }

        /// <summary>
        ///     A <see cref="StatusReference" /> is constructed from the url acquired from
        ///     <see cref="DirectJobResponse.ResponseUrls" />, and a token provided as a
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