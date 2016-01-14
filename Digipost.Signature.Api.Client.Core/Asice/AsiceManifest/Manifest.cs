using System;
using System.Collections.Generic;
using System.Text;
using Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Core.Asice.AsiceManifest
{
    public class Manifest : IAsiceAttachable
    {
        public Sender Sender { get; }
        public Document Document { get; }
        public IEnumerable<Signer> Signers { get; }

        public Manifest(Sender sender, Document document, IEnumerable<Signer> signers)
        {
            Sender = sender;
            Document = document;
            Signers = signers;
        }

        public byte[] Bytes
        {
            get
            {
                var manifestDataTranferObject = DataTransferObjectConverter.ToDataTransferObject(this);
                var serializedManifest = SerializeUtility.Serialize(manifestDataTranferObject);

                return Encoding.UTF8.GetBytes(serializedManifest);
            }
        }

        public string Id
        {
            get { return "Id_1"; }
        }

        public string FileName
        {
            get { return "manifest.xml"; }
        }

        public string MimeType
        {
            get { return "application/xml"; }
        }
    }
}
