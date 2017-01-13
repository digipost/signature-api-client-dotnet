using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Internal.Asice.AsiceSignature;

namespace Digipost.Signature.Api.Client.Portal.Internal.AsicE
{
    internal class PortalAsiceGenerator : AsiceGenerator
    {
        public static DocumentBundle CreateAsice(Job job, X509Certificate2 certificate, IAsiceConfiguration asiceConfiguration)
        {
            var manifest = new Manifest(job.Sender, (Document) job.Document, job.Signers)
            {
                Availability = job.Availability,
                AuthenticationLevel = job.AuthenticationLevel
            };
            var signature = new SignatureGenerator(certificate, job.Document, manifest);

            var asiceArchive = GetAsiceArchive(job, asiceConfiguration, job.Document, manifest, signature);

            return new DocumentBundle(asiceArchive.GetBytes());
        }
    }
}