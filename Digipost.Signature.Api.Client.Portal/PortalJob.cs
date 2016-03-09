using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalJob : IRequestContent
    {
        public PortalJob(Document document, IEnumerable<Signer> signers, string reference, Sender sender = null)
        {
            Document = document;
            Signers = signers;
            Reference = reference;
            Sender = sender;
        }

        public Document Document { get; }

        public IEnumerable<Signer> Signers { get; }

        public string Reference { get; }

        public Sender Sender { get; }

        public DateTime DistributionTime { get; }
    }
}