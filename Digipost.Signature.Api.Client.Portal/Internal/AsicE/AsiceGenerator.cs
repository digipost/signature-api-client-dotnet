using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;

namespace Digipost.Signature.Api.Client.Portal.Internal.AsicE
{
    internal class AsiceGenerator
    {
        public static DocumentBundle CreateAsice(Sender sender, Document document, IEnumerable<Signer> signers, Availability availability, X509Certificate2 certificate)
        {
            var manifest = new PortalManifest(sender, document, signers)
            {
                Availability = availability
            };

            var signature = new SignatureGenerator(certificate, document, manifest);
            var asiceArchive = new AsiceArchive(document, signature, manifest);

            return new DocumentBundle(asiceArchive.Bytes);
        }
    }
}