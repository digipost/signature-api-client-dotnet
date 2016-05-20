using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Job : IRequestContent, ISignatureJob
    {
        public Job(Document document, AbstractSigner signer, string reference, ExitUrls exitUrls, Sender sender = null)
        {
            Reference = reference;
            Signer = signer;
            Document = document;
            ExitUrls = exitUrls;
            Sender = sender;
        }

        public AbstractSigner Signer { get; }

        public ExitUrls ExitUrls { get; }

        public string Reference { get; }

        public Document Document { get; }

        public Sender Sender { get; internal set; }
    }
}