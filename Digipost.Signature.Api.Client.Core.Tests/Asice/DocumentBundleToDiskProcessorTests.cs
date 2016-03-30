using System;
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
            [TestClass]
            public class ConstructorMethod : DocumentBundleToDiskProcessorTests
            {
                [TestMethod]
                public void SimpleConstructor()
                {
                    //Arrange
                    var directory = "C:\\directory";

                    //Act
                    var documentBundleToDiskProcessor = new DocumentBundleToDiskProcessor(directory);

                    //Assert
                    Assert.AreEqual(directory, documentBundleToDiskProcessor.Directory);
                }
            }

            [TestMethod]
            public void PersistsFileToDisk()
            {
                //Arrange
                var tmpDirectory = Path.GetTempPath();
                var processor = new DocumentBundleToDiskProcessor(tmpDirectory);
                var mockSignatureJob = new SignatureJobStub() { Reference = "AReference"};
                var documentBytes = CoreDomainUtility.GetDocument().Bytes;
                var fileStream = new MemoryStream(documentBytes);

                //Act
                processor.Process(mockSignatureJob, fileStream);
                var processedFileName = processor.LastFileProcessed;
                var tempFile = Path.Combine(tmpDirectory, processedFileName);

                //Assert
                Assert.AreEqual(documentBytes.Length, new FileInfo(tempFile).Length);
            }

            [TestMethod]
            public void FileNameContainsEssentialData()
            {
                //Arrange
                var tmpDirectory = Path.GetTempPath();
                var fileEnding = "asice.zip";

                var processor = new DocumentBundleToDiskProcessor(tmpDirectory);
                var mockSignatureJob = new SignatureJobStub() { Reference = "AReference" };
                var documentBytes = CoreDomainUtility.GetDocument().Bytes;
                var fileStream = new MemoryStream(documentBytes);

                //Act
                processor.Process(mockSignatureJob, fileStream);
                var processedFileName = processor.LastFileProcessed;
                var tempFile = Path.Combine(tmpDirectory, processedFileName);

                //Assert
                Assert.IsTrue(tempFile.Contains(tmpDirectory));
                Assert.IsTrue(tempFile.Contains(fileEnding));
                Assert.IsTrue(tempFile.Contains(mockSignatureJob.Reference));
                Assert.IsTrue(tempFile.Contains(DateTime.Now.Year.ToString()));
            }
        }
    }
}