namespace Digipost.Signature.Api.Client.Core.Asice
{
    public class DocumentBundle
    {
        public byte[] BundleBytes { get; internal set; }

        public DocumentBundle(byte[] bundleBytes)
        {
            BundleBytes = bundleBytes;
        }
    }
}