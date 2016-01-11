using System.Collections.Generic;

namespace Digipost.Signature.Api.Client.Core.Asice.AsiceManifest
{
    public class Manifest : IAsiceAttachable
    {
        public Manifest(Sender sender, Document document, IEnumerable<Signer> signers)
        {
            
        }

        public Manifest(byte[] bytes)
        {
            Bytes = bytes;
        }

        public byte[] Bytes { get; }

        public FileType FileType { get; }

        public string Id { get; }

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
