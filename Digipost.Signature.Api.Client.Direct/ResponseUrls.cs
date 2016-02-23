using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class ResponseUrls
    {
        public ResponseUrls(Uri redirectUrl, Uri statusUrl)
        {
            Redirect = new RedirectReference(redirectUrl);
            Status = new StatusReference(statusUrl);
        }

        public RedirectReference Redirect { get; set; }

        public StatusReference Status { get; set; }
    }
}