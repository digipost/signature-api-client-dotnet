using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class ResponseUrls
    {
        private readonly Uri _statusUrl;

        public ResponseUrls(Uri redirectUrl, Uri statusUrl)
        {
            _statusUrl = statusUrl;
            Redirect = new RedirectReference(redirectUrl);
        }

        public RedirectReference Redirect { get; set; }

        public StatusReference Status(string statusQueryToken)
        {
            return new StatusReference(_statusUrl, statusQueryToken);
        }
    }
}