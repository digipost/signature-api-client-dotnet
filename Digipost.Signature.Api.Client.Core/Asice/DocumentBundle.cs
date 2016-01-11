namespace Digipost.Signature.Api.Client.Direct
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