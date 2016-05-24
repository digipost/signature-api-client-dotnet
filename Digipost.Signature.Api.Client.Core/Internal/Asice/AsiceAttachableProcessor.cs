using System.IO;

namespace Digipost.Signature.Api.Client.Core.Internal.Asice
{
    internal class AsiceAttachableProcessor
    {
        private readonly IDocumentBundleProcessor _documentBundleProcessor;
        private readonly ISignatureJob _signatureJob;

        public AsiceAttachableProcessor(ISignatureJob signatureJob, IDocumentBundleProcessor documentBundleProcessor)
        {
            _signatureJob = signatureJob;
            _documentBundleProcessor = documentBundleProcessor;
        }

        public void Process(Stream stream)
        {
            _documentBundleProcessor.Process(_signatureJob, stream);
        }
    }
}