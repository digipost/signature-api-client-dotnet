using System.IO;

namespace Digipost.Signature.Api.Client.Core.Asice
{
    public class DocumentBundleToDiskProcessor : IDocumentBundleProcessor
    {
        public string Path { get; }

        public DocumentBundleToDiskProcessor(string path)
        {
            Path = path;
        }

        public void Process(ISignatureJob signatureJob, Stream bundleStream)
        {
            using (var fileStream = File.Create(Path))
            {
                bundleStream.CopyTo(fileStream);
            }
        }
    }
}
