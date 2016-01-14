using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Direct;

namespace Digipost.Signature.Api.Client.Core.Asice
{
    internal class AsiceGenerator
    {
        public static DocumentBundle CreateAsice(Sender sender, Document document, IEnumerable<Signer> signers, X509Certificate2 certificate)
        {
            var manifest = new Manifest(sender, document, signers);
            
            var attachables = new List<IAsiceAttachable> {manifest, document};

            var signature = new AsiceSignature.Signature(document, manifest, certificate);
            

            var result = signature.Bytes;
            

            //Zip it and create Documentbundle with bytes.

            throw new NotImplementedException();
        }
    }
}
