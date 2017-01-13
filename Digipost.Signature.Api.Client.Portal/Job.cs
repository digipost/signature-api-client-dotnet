using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Portal
{
    public class Job : IRequestContent, ISignatureJob
    {
        public Job(Document document, IEnumerable<Signer> signers, string reference, Sender sender = null)
        {
            Document = document;
            Signers = signers;
            Reference = reference;
            Sender = sender;
        }

        public IEnumerable<Signer> Signers { get; }

        public Availability Availability { get; set; }

        public AuthenticationLevel? AuthenticationLevel { get; set; }

        public AbstractDocument Document { get; }

        public string Reference { get; }

        public Sender Sender { get; internal set; }

        public override string ToString()
        {
            return $"Signers: {Signers}, Availability: {Availability}, Document: {Document}, Reference: {Reference}, Sender: {Sender}";
        }
    }
}