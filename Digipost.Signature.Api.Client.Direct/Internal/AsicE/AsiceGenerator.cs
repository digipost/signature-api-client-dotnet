using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;

namespace Digipost.Signature.Api.Client.Direct.Internal.AsicE
{
    internal class AsiceGenerator
    {
        public static DocumentBundle CreateAsice(Sender sender, Document document, Signer signer, X509Certificate2 certificate)
        {
            var manifest = new DirectManifest(sender, document, signer);
            var signature = new SignatureGenerator(certificate, document, manifest);
            
            var asiceArchive = new AsiceArchive(document, signature, manifest);

            return new DocumentBundle(asiceArchive.Bytes);
        }
    }
}
