using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core.Enums;

namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    public class SignatureJobStub : ISignatureJob
    {
        public Sender Sender { get; }

        public IEnumerable<Document> Documents { get; }
        
        public string Reference { get; set; }

        public string Title { get; }

        public string Description { get; }

        public AuthenticationLevel? AuthenticationLevel { get; set; }

        public IdentifierInSignedDocuments? IdentifierInSignedDocuments { get; set; }
    }
}
