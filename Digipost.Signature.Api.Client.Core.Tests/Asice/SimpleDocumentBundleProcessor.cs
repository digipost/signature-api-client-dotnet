using System.IO;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice
{
    public class SimpleDocumentBundleProcessor : IDocumentBundleProcessor
    {
        public long StreamLength { get; private set; }

        public long Initialposition { get; private set; }

        public bool CouldReadBytesStream { get; private set; }

        public void Process(ISignatureJob signatureJob, Stream bundleStream)
        {
            Initialposition = bundleStream.Position;

            CouldReadBytesStream = bundleStream.CanRead;
            StreamLength = bundleStream.Length;

            DoSomeStreamMessing(bundleStream);
        }

        private void DoSomeStreamMessing(Stream bundleStream)
        {
            bundleStream.Position = 100;
        }
    }
}
