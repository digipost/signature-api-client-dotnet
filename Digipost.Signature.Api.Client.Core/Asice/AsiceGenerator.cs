using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;

namespace Digipost.Signature.Api.Client.Core.Asice
{
    internal class AsiceGenerator
    {
        public static DocumentBundle CreateAsice(Sender sender, Document document, IEnumerable<Signer> signers, X509Certificate2 certificate)
        {
            var manifest = new Manifest(sender, document, signers);
            var signature = new SignatureGenerator(certificate, document, manifest);
            
            var asiceArchive = new AsiceArchive(document, signature, manifest);

            return new DocumentBundle(asiceArchive.Bytes);
        }
    }
}
