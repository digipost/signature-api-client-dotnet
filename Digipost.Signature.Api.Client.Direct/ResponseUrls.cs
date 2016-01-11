using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class ResponseUrls
    {
        public Uri Redirect { get; set; }
        public Uri Status { get; set; }

        public ResponseUrls(Uri redirectUrl, Uri statusUrl)
        {
            Redirect = redirectUrl;
            Status = statusUrl;
        }
    }
}
