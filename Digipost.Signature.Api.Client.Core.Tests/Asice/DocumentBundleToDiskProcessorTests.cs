using System;
using System.IO;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice
{
    public class DocumentBundleToDiskProcessorTests
    {
        public class ProcessMethod : DocumentBundleToDiskProcessorTests
        {
            public class ConstructorMethod : DocumentBundleToDiskProcessorTests
            {
                [Fact]
                public void Simple_constructor()
                {
                    //Arrange
                    var directory = "C:\\directory";

                    //Act
                    var documentBundleToDiskProcessor = new DocumentBundleToDiskProcessor(directory);

                    //Assert
                    Assert.Equal(directory, documentBundleToDiskProcessor.Directory);
                }
            }

            [Fact]
            public void File_name_contains_essential_data()
            {
                //Arrange
                var tmpDirectory = Path.GetTempPath();
                var fileEnding = "asice.zip";

                var processor = new DocumentBundleToDiskProcessor(tmpDirectory);
                var mockSignatureJob = new SignatureJobStub {Reference = "AReference"};
                var documentBytes = CoreDomainUtility.GetDocument().Bytes;
                var fileStream = new MemoryStream(documentBytes);

                //Act
                processor.Process(mockSignatureJob, fileStream);
                var processedFileName = processor.LastFileProcessed;
                var tempFile = Path.Combine(tmpDirectory, processedFileName);

                //Assert
                Assert.Contains(tmpDirectory, tempFile);
                Assert.Contains(fileEnding, tempFile);
                Assert.Contains(mockSignatureJob.Reference, tempFile);
                Assert.Contains(DateTime.Now.Year.ToString(), tempFile);
            }

            [Fact]
            public void Persists_file_to_disk()
            {
                //Arrange
                var tmpDirectory = Path.GetTempPath();
                var processor = new DocumentBundleToDiskProcessor(tmpDirectory);
                var mockSignatureJob = new SignatureJobStub {Reference = "AReference"};
                var documentBytes = CoreDomainUtility.GetDocument().Bytes;
                var fileStream = new MemoryStream(documentBytes);

                //Act
                processor.Process(mockSignatureJob, fileStream);
                var processedFileName = processor.LastFileProcessed;
                var tempFile = Path.Combine(tmpDirectory, processedFileName);

                //Assert
                Assert.Equal(documentBytes.Length, new FileInfo(tempFile).Length);
            }
        }
    }
}
