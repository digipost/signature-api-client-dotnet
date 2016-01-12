using System;
using System.Collections.Generic;

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
                throw new NotImplementedException();
            }
        }

        public string Id
        {
            get
            {
                throw new NotImplementedException();
            }
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
