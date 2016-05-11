using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;

namespace Digipost.Signature.Api.Client.Portal.Internal.AsicE
{
    internal class PortalAsiceGenerator : AsiceGenerator
    {
        public static DocumentBundle CreateAsice(PortalJob portalJob, X509Certificate2 certificate, IAsiceConfiguration asiceConfiguration)
        {
            var manifest = new PortalManifest(portalJob.Sender, (PortalDocument) portalJob.Document, portalJob.Signers)
            {
                Availability = portalJob.Availability
            };
            var signature = new SignatureGenerator(certificate, portalJob.Document, manifest);

            var asiceArchive = GetAsiceArchive(portalJob, asiceConfiguration, portalJob.Document, manifest, signature);

            return new DocumentBundle(asiceArchive.GetBytes());
        }
    }
}