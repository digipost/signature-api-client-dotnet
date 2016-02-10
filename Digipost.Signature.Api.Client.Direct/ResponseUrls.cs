using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class ResponseUrls
    {
        public RedirectReference Redirect { get; set; }
        public StatusReference Status { get; set; }

        public ResponseUrls(Uri redirectUrl, Uri statusUrl)
        {
            Redirect = new RedirectReference(redirectUrl);
            Status = new StatusReference(statusUrl);
        }
    }
}
