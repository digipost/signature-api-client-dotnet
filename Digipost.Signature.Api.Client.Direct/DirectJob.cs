using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJob : IRequestContent
    {
        public Sender Sender { get; private set; }
        public string Reference { get; private set; }
        public Signer Signer { get; private set; }
        public Document Document { get; private set; }
        public ExitUrls ExitUrls { get; private set; }

        public DirectJob(Sender sender, Document document, Signer signer, string reference, ExitUrls exitUrls)
        {
            Sender = sender;
            Reference = reference;
            Signer = signer;
            Document = document;
            ExitUrls = exitUrls;
        }
    }
}
