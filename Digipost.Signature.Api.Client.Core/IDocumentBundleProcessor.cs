using System.IO;

namespace Digipost.Signature.Api.Client.Core
{
    public interface IDocumentBundleProcessor
    {
        void Process(ISignatureJob signatureJob, Stream bundleStream);
    }
}