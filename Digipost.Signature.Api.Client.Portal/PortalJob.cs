using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalJob : IRequestContent, ISignatureJob
    {
        public PortalJob(PortalDocument document, IEnumerable<PortalSigner> signers, string reference, Sender sender = null)
        {
            Document = document;
            Signers = signers;
            Reference = reference;
            Sender = sender;
        }

        public IEnumerable<PortalSigner> Signers { get; }

        public Availability Availability { get; set; }

        public Document Document { get; }

        public string Reference { get; }

        public Sender Sender { get; internal set; }
    }
}