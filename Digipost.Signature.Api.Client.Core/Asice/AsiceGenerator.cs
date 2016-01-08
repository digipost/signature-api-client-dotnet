using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Digipost.Signature.Api.Client.Core.Asice
{
    internal class AsiceGenerator
    {
        public static void CreateAsice(Document document, IEnumerable<Signer> signers, Sender sender,
            X509Certificate2 certificate)
        {
            //Create Manifest

            //Create list of AsiceFiles (manifest and document)

            //Create Signature

            //Zip it and create Documentbundle with bytes.
        }
    }
}
