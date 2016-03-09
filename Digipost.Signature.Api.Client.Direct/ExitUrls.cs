using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class ExitUrls
    {
        public ExitUrls(Uri completionUrl, Uri rejectionUrl, Uri errorUrl)
        {
            CompletionUrl = completionUrl;
            RejectionUrl = rejectionUrl;
            ErrorUrl = errorUrl;
        }

        public Uri CompletionUrl { get; set; }

        public Uri RejectionUrl { get; set; }

        public Uri ErrorUrl { get; set; }
    }
}