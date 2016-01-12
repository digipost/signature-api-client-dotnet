using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Direct;

namespace Digipost.Signature.Api.Client.Core.Asice
{
    internal class AsiceGenerator
    {
        public static DocumentBundle CreateAsice(Sender sender, Document document, IEnumerable<Signer> signers, X509Certificate2 certificate)
        {
            //Create Manifest
            ManifestGenerator.GenerateManifestBytes(new Manifest(sender, document, signers));

            //Create list of AsiceFiles (manifest and document)

            //Create Signature

            //Zip it and create Documentbundle with bytes.

            throw new NotImplementedException();
        }
    }
}
