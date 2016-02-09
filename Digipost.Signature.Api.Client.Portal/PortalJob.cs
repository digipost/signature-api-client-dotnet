using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Portal
{

    public class PortalJob : IRequestContent
    {
        public Document Document { get; set; }

        public string Reference { get; private set; }

        public IEnumerable<Signer> Signers { get; private set; }

        public DateTime DistributionTime { get; private set; }

        public PortalJob(Document document, IEnumerable<Signer> signers, string reference)
        {
            Document = document;
            Signers = signers;
            Reference = reference;
        }
    }
}