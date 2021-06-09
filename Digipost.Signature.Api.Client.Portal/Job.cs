using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Portal
{
    public class Job : IRequestContent, ISignatureJob
    {
        public Job(string title, IEnumerable<Document> documents, IEnumerable<Signer> signers, string reference, Sender sender = null)
        {
            Title = title;
            Documents = documents;
            Signers = signers;
            Reference = reference;
            Sender = sender;
        }

        public IEnumerable<Signer> Signers { get; }

        public Availability Availability { get; set; }

        public AuthenticationLevel? AuthenticationLevel { get; set; }

        public IdentifierInSignedDocuments? IdentifierInSignedDocuments { get; set; }

        public IEnumerable<Document> Documents { get; }

        public string Reference { get; }

        public string Title { get; }
        
        public string NonSensitiveTitle { get; set; }

        public string Description { get; set; }

        public Sender Sender { get; internal set; }

        public override string ToString()
        {
            return $"Title:{Title} NonSensitiveTitle:{NonSensitiveTitle} Description:{Description} Signers: {Signers}, Availability: {Availability}, Documents: {Documents}, Reference: {Reference}, Sender: {Sender}";
        }
    }
}
