using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;

namespace Digipost.Signature.Api.Client.Direct.Internal.AsicE
{
    internal class AsiceGenerator
    {
        public static DocumentBundle CreateAsice(DirectJob directJob, X509Certificate2 certificate, IAsiceConfiguration asiceConfiguration = null)
        {
            var manifest = new DirectManifest(directJob.Sender, directJob.Document, directJob.Signer);
            var signature = new SignatureGenerator(certificate, directJob.Document, manifest);

            var asiceArchive = GetAsiceArchive(directJob, asiceConfiguration, directJob.Document, signature, manifest);

            return new DocumentBundle(asiceArchive.GetBytes());
        }

        private static AsiceArchive GetAsiceArchive(ISignatureJob signatureJobForMetadata, IAsiceConfiguration asiceConfiguration, params IAsiceAttachable[] asiceAttachables)
        {
            return  asiceConfiguration == null 
                ? new AsiceArchive(asiceAttachables) 
                : new AsiceArchive(asiceConfiguration.DocumentBundleProcessors, signatureJobForMetadata, asiceAttachables);
        }
    }
}