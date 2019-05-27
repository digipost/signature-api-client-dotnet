using System;
using Digipost.Signature.Api.Client.Direct.NewRedirectUrl;

namespace Digipost.Signature.Api.Client.Direct
{
    public class NewRedirectUrlRequest : IWithSignerUrl
    {
        public static NewRedirectUrlRequest FromSignerUrl(Uri signerUrl)
        {
            return new NewRedirectUrlRequest(signerUrl);
        }

        private NewRedirectUrlRequest(Uri signerUrl)
        {
            SignerUrl = signerUrl;
        }

        public Uri SignerUrl { get; }
    }
}
