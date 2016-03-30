using System.IO;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice
{
    [TestClass()]
    public class DocumentBundleToDiskProcessorTests
    {
        [TestClass]
        public class ProcessMethod : DocumentBundleToDiskProcessorTests
        {
            [TestMethod]
            public void PersistsFileToDisk()
            {
                //Arrange
                var tempFile = Path.GetTempFileName();
                var processor = new DocumentBundleToDiskProcessor(tempFile);
                var mockSignatureJob = new SignatureJobStub() { Reference = "AReference"};
                var documentBytes = CoreDomainUtility.GetDocument().Bytes;
                var fileStream = new MemoryStream(documentBytes);

                //Act
                processor.Process(mockSignatureJob, fileStream);

                //Assert
                Assert.AreEqual(documentBytes.Length, new FileInfo(tempFile).Length);
            }
        }
    }
}