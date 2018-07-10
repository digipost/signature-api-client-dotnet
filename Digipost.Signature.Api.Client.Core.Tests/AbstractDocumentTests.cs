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
                var executablePath = Path.GetDirectoryName(executingAssembly.CodeBase);
                var documentPath = Path.Combine(executablePath, "Resources", "Documents", "Dokument.pdf");
                return new Uri(documentPath).LocalPath;
            }

            private static string DocumentFilePath()
            {
                var executingAssembly = Assembly.GetExecutingAssembly();
                var executablePath = Path.GetDirectoryName(executingAssembly.CodeBase);
                var documentPath = Path.Combine(executablePath, "Resources", "Documents", "Dokument.pdf");
                return new Uri(documentPath).AbsolutePath;
            }

            [Fact]
            public void Initializes_all_values_with_document_bytes()
            {
                //Arrange
                const string subject = "subject";
                const string message = "message";
                const FileType fileType = FileType.Pdf;
                const string expectedMimeType = "application/pdf";

                var pdfDocumentBytes = CoreDomainUtility.GetPdfDocumentBytes();

                //Act
                var document = new DocumentStub(
                    subject,
                    message,
                    fileType,
                    pdfDocumentBytes
                );

                //Assert
                Assert.Equal(subject, document.Title);
                Assert.Equal(message, document.Message);
                Assert.Equal(expectedMimeType, document.MimeType);
                Assert.True(pdfDocumentBytes.SequenceEqual(document.Bytes));
            }

            [Fact]
            public void Initializes_all_values_with_document_path()
            {
                //Arrange
                const string subject = "subject";
                const string message = "message";
                const FileType fileType = FileType.Txt;
                const string expectedMimeType = "text/plain";

                var documentPath = DocumentFilePath();

                //Act
                var document = new DocumentStub(
                    subject,
                    message,
                    fileType,
                    documentPath
                );

                var pdfDocumentBytes = File.ReadAllBytes(documentPath);

                //Assert
                Assert.Equal(subject, document.Title);
                Assert.Equal(message, document.Message);
                Assert.Equal(expectedMimeType, document.MimeType);
                Assert.True(pdfDocumentBytes.SequenceEqual(document.Bytes));
            }
        }

        public class IdMethod : AbstractDocumentTests
        {
            [Fact]
            public void Returns_correct_static_string()
            {
                //Arrange
                var id = "Id_0";

                //Act
                var document = CoreDomainUtility.GetDocument();

                //Assert
                Assert.Equal(id, document.Id);
            }
        }

        public class FileNameMethod : AbstractDocumentTests
        {
            [Fact]
            public void Returns_file_name_with_date()
            {
                //Arrange

                //Act
                var document = new DocumentStub("title", "message", FileType.Txt, new byte[] {0xb});

                //Assert
                Assert.True(document.FileName.Contains(DateTime.Now.ToString("yyyyMMdd")) && document.FileName.Contains("document"));
            }
        }
    }
}