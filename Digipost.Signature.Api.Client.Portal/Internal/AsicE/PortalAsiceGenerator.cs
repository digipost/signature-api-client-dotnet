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
            var manifest = ToManifest(job);

            var signature = new SignatureGenerator(certificate, job.Documents, manifest);
            var asiceArchive = GetAsiceArchive(job, asiceConfiguration, job.Documents, manifest, signature);

            return new DocumentBundle(asiceArchive.GetBytes());
        }

        internal static Manifest ToManifest(Job job)
        {
            return new Manifest(job.Title, job.Sender, job.Documents, job.Signers)
            {
                Availability = job.Availability,
                AuthenticationLevel = job.AuthenticationLevel,
                IdentifierInSignedDocuments = job.IdentifierInSignedDocuments,
                Description = job.Description,
                NonSensitiveTitle = job.NonSensitiveTitle
            };
        }
    }

}
