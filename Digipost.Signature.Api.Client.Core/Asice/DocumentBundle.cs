namespace Digipost.Signature.Api.Client.Core.Asice
{
    public class DocumentBundle
    {
        public DocumentBundle(byte[] bundleBytes)
        {
            BundleBytes = bundleBytes;
        }

        public byte[] BundleBytes { get; internal set; }
    }
}