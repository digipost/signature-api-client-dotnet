using System;
using System.IO;
using Digipost.Signature.Api.Client.Core.Internal.Utilities;

namespace Digipost.Signature.Api.Client.Core.Internal.Asice
{
    public class DocumentBundleToDiskProcessor : IDocumentBundleProcessor
    {
        public DocumentBundleToDiskProcessor(string directory)
        {
            Directory = directory;
        }

        public string Directory { get; }

        public string LastFileProcessed { get; set; }

        public void Process(ISignatureJob signatureJob, Stream bundleStream)
        {
            LastFileProcessed = FileNameWithTimeStamp(signatureJob.Reference);
            using (var fileStream = File.Create(Path.Combine(Directory, LastFileProcessed)))
            {
                bundleStream.CopyTo(fileStream);
            }
        }

        private static string FileNameWithTimeStamp(string reference)
        {
            return $"{DateTime.Now.ToString(DateUtility.DateForFile())} - {reference}.asice.zip";
        }
    }
}
