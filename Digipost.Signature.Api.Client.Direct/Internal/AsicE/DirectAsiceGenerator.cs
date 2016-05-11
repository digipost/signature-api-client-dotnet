using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;

namespace Digipost.Signature.Api.Client.Direct.Internal.AsicE
{
    internal class DirectAsiceGenerator : AsiceGenerator
    {
        public static DocumentBundle CreateAsice(DirectJob directJob, X509Certificate2 certificate, IAsiceConfiguration asiceConfiguration)
        {
            var manifest = new DirectManifest(directJob.Sender, (DirectDocument) directJob.Document, directJob.Signer);
            var signature = new SignatureGenerator(certificate, directJob.Document, manifest);

            var asiceArchive = GetAsiceArchive(directJob, asiceConfiguration, directJob.Document, signature, manifest);

            return new DocumentBundle(asiceArchive.GetBytes());
        }
    }
}