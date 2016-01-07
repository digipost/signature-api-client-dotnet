namespace Digipost.Signature.Api.Client.Asice
{
    public class Manifest : IAsiceAttachable
    {
        public Manifest(byte[] bytes)
        {
            Bytes = bytes;
        }

        public byte[] Bytes { get; }

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
