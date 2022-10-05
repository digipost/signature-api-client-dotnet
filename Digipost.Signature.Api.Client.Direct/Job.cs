using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Job : IRequestContent, ISignatureJob
    {
        public Job(string title, IEnumerable<Document> documents, IEnumerable<Signer> signers, string reference, ExitUrls exitUrls, Sender sender = null, StatusRetrievalMethod statusRetrievalMethod = StatusRetrievalMethod.WaitForCallback)
        {
            Title = title;
            Reference = reference;
            Signers = signers;
            Documents = documents;
            ExitUrls = exitUrls;
            Sender = sender;
            StatusRetrievalMethod = statusRetrievalMethod;
        }

        public IEnumerable<Signer> Signers { get; }

        public ExitUrls ExitUrls { get; }

        public StatusRetrievalMethod StatusRetrievalMethod { get; }

        public AuthenticationLevel? AuthenticationLevel { get; set; }

        public IdentifierInSignedDocuments? IdentifierInSignedDocuments { get; set; }

        public string Reference { get; }

        public string Title { get; }

        public string Description { get; set; }

        public IEnumerable<Document> Documents { get; }

        public Sender Sender { get; internal set; }

        public override string ToString()
        {
            return $"Title:{Title} Description:{Description} Signers: {Signers}, ExitUrls: {ExitUrls}, Reference: {Reference}, Documents: {Documents}, Sender: {Sender}, Retrieving status by {StatusRetrievalMethod}";
        }
    }
}
