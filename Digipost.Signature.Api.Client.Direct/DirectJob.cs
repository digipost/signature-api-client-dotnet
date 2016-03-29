using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJob : IRequestContent, ISignatureJob
    {
        public DirectJob(Document document, Signer signer, string reference, ExitUrls exitUrls, Sender sender = null)
        {
            Reference = reference;
            Signer = signer;
            Document = document;
            ExitUrls = exitUrls;
            Sender = sender;
        }

        public string Reference { get; }

        public Signer Signer { get; }

        public Document Document { get; }

        public ExitUrls ExitUrls { get; }

        public Sender Sender { get; internal set; }
    }
}