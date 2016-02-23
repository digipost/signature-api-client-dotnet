using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJob : IRequestContent
    {
        public DirectJob(Document document, Signer signer, string reference, ExitUrls exitUrls)
        {
            Reference = reference;
            Signer = signer;
            Document = document;
            ExitUrls = exitUrls;
        }

        public string Reference { get; private set; }

        public Signer Signer { get; private set; }

        public Document Document { get; private set; }

        public ExitUrls ExitUrls { get; private set; }
    }
}