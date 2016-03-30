using System;
using System.IO;
using Digipost.Signature.Api.Client.Core.Utilities;

namespace Digipost.Signature.Api.Client.Core.Asice
{
    public class DocumentBundleToDiskProcessor : IDocumentBundleProcessor
    {
        public string Directory { get; }

        public string LastFileProcessed { get; set; }

        public DocumentBundleToDiskProcessor(string directory)
        {
            Directory = directory;
        }

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
