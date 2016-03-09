using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class StatusReference
    {
        public string StatusQueryToken { get;}
        internal Uri BaseUrl { get; }

        public StatusReference(Uri baseUrl, string statusQueryToken)
        {
            BaseUrl = baseUrl;
            StatusQueryToken = statusQueryToken;
        }

        public Uri Url()
        {
           return new Uri($"{BaseUrl.AbsoluteUri}?status_query_token={StatusQueryToken}");
        }
    }
}