using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
