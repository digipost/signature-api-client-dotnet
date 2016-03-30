using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalJob : IRequestContent, ISignatureJob
    {
        public PortalJob(Document document, IEnumerable<Signer> signers, string reference, Sender sender = null)
        {
            Document = document;
            Signers = signers;
            Reference = reference;
            Sender = sender;
        }

        public IEnumerable<Signer> Signers { get; }

        public Availability Availability { get; set; }

        public Document Document { get; }

        public string Reference { get; }

        public Sender Sender { get; internal set; }
    }
}