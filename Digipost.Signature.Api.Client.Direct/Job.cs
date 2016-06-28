using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Job : IRequestContent, ISignatureJob
    {
        public Job(Document document, Signer signer, string reference, ExitUrls exitUrls, Sender sender = null, StatusRetrievalMethod statusRetrievalMethod = StatusRetrievalMethod.WaitForCallback)
        {
            Reference = reference;
            Signer = signer;
            Document = document;
            ExitUrls = exitUrls;
            Sender = sender;
            StatusRetrievalMethod = statusRetrievalMethod;
        }

        public Signer Signer { get; }

        public ExitUrls ExitUrls { get; }

        public string Reference { get; }

        public AbstractDocument Document { get; }

        public Sender Sender { get; internal set; }

        public StatusRetrievalMethod StatusRetrievalMethod { get; }

        public override string ToString()
        {
            return $"Signer: {Signer}, ExitUrls: {ExitUrls}, Reference: {Reference}, Document: {Document}, Sender: {Sender}, Retrieving status bu {StatusRetrievalMethod}";
        }
    }
}