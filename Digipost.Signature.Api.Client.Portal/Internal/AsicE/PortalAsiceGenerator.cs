using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;

namespace Digipost.Signature.Api.Client.Portal.Internal.AsicE
{
    internal class PortalAsiceGenerator : AsiceGenerator
    {
        public static DocumentBundle CreateAsice(Job job, X509Certificate2 certificate, IAsiceConfiguration asiceConfiguration)
        {
            var manifest = new PortalManifest(job.Sender, (Document) job.Document, job.Signers)
            {
                Availability = job.Availability
            };
            var signature = new SignatureGenerator(certificate, job.Document, manifest);

            var asiceArchive = GetAsiceArchive(job, asiceConfiguration, job.Document, manifest, signature);

            return new DocumentBundle(asiceArchive.GetBytes());
        }
    }
}