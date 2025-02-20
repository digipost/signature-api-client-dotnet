using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class AbstractDocumentTests
    {
        public class ConstructorMethod : AbstractDocumentTests
        {
            private static string Document_file_path()
            {
                var executingAssembly = Assembly.GetExecutingAssembly();
                var executablePath = Path.GetDirectoryName(executingAssembly.Location);
                var documentPath = Path.Combine(executablePath, "Resources", "Documents", "Dokument.pdf");
                return new Uri(documentPath).LocalPath;
            }

            [Fact]
            public void Initializes_all_values_with_document_bytes()
            {
                //Arrange
                const string title = "documentTitle";
                const FileType fileType = FileType.Pdf;
                const string expectedMimeType = "application/pdf";

                var pdfDocumentBytes = CoreDomainUtility.GetPdfDocumentBytes();

                //Act
                var document = new DocumentStub(
                    title,
                    fileType,
                    pdfDocumentBytes
                );

                //Assert
                Assert.Equal(title, document.Title);
                Assert.Equal(expectedMimeType, document.MimeType);
                Assert.True(pdfDocumentBytes.SequenceEqual(document.Bytes));
            }

            [Fact]
            public void Initializes_all_values_with_document_path()
            {
                //Arrange
                const string title = "documentTitle";
                const FileType fileType = FileType.Txt;
                const string expectedMimeType = "text/plain";

                var documentPath = DocumentFilePath();

                //Act
                var document = new DocumentStub(
                    title,
                    fileType,
                    documentPath
                );

                var pdfDocumentBytes = File.ReadAllBytes(documentPath);

                //Assert
                Assert.Equal(title, document.Title);
                Assert.Equal(expectedMimeType, document.MimeType);
                Assert.True(pdfDocumentBytes.SequenceEqual(document.Bytes));
            }

            private static string DocumentFilePath()
            {
                var directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName;
                var documentPath = Path.Combine(directoryInfo, "Resources", "Documents", "Dokument.pdf").Replace("file:", "");

                return documentPath;
            }
        }
    }
}
