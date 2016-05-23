using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class StatusReference
    {
        public static string StatusQueryTokenParamName = "status_query_token";

        /// <summary>
        ///     A <see cref="StatusReference" /> is constructed from the url acquired from
        ///     <see cref="JobResponse.ResponseUrls" />, and a token provided as a
        ///     <see cref="StatusQueryTokenParamName">query parameter</see> which is appended to the
        ///     <see cref="ExitUrls">exit Url</see>, which the signer is redirected to when the signing ceremony is
        ///     completed/aborted/failed. The token needs to be consumed by the system the user is redirected to, and consequently
        ///     provided to the constructor and passed to <see cref="DirectClient.GetStatus" />.
        /// </summary>
        /// <remarks>
        ///     The status will usually be retrieved from <see cref="JobResponse.ResponseUrls" />. Only use this
        ///     constructor if, for some reason, you need to create the reference manually.
        /// </remarks>
        public StatusReference(Uri baseUrl, string statusQueryToken)
        {
            BaseUrl = baseUrl;
            StatusQueryToken = statusQueryToken;
        }

        public string StatusQueryToken { get; }

        internal Uri BaseUrl { get; }

        public override string ToString()
        {
            return $"StatusQueryToken: {StatusQueryToken}, BaseUrl: {BaseUrl}";
        }

        public Uri Url()
        {
            return new Uri($"{BaseUrl.AbsoluteUri}?{StatusQueryTokenParamName}={StatusQueryToken}");
        }
    }
}