using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class ExitUrls
    {
        public Uri CompletionUrl { get; set; }
        public Uri CancellationUrl { get; set; }
        public Uri ErrorUrl { get; set; }

        public ExitUrls(Uri completionUrl, Uri cancellationUrl, Uri errorUrl)
        {
            CompletionUrl = completionUrl;
            CancellationUrl = cancellationUrl;
            ErrorUrl = errorUrl;
        }
    }
}
