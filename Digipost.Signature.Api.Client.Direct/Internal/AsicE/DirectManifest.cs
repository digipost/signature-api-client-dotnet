﻿using System.Text;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Direct.Internal.AsicE
{
    internal class DirectManifest : IAsiceAttachable
    {
        public Sender Sender { get; internal set; }
        public Document Document { get; internal set; }
        public Signer Signer { get; internal set; }

        public DirectManifest(Sender sender, Document document, Signer signer)
        {
            Sender = sender;
            Document = document;
            Signer = signer;
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