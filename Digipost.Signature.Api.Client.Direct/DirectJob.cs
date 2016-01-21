using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJob : IRequestContent
    {
        public Sender Sender { get; set; }
        public string Reference { get; }
        public Signer Signer { get; }
        public Document Document { get; }
        public ExitUrls ExitUrls { get; }

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
