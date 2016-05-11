using System.IO;
using System.Linq;
using System.Reflection;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class DocumentTests
    {
        [TestClass]
        public class ConstructorMethod : DocumentTests
        {
            [TestMethod]
            public void InitializesAllValuesWithDocumentBytes()
            {
                //Arrange
                const string subject = "subject";
                const string message = "message";
                const string fileName = "NavnPåFil";
                const FileType fileType = FileType.Pdf;
                const string expectedMimeType = "application/pdf";

                var pdfDocumentBytes = CoreDomainUtility.GetPdfDocumentBytes();

                //Act
                var document = new DocumentStub(
                    subject,
                    message,
                    fileName,
                    fileType,
                    pdfDocumentBytes
                    );

                //Assert
                Assert.AreEqual(subject, document.Title);
                Assert.AreEqual(message, document.Message);
                Assert.AreEqual(fileName, document.FileName);
                Assert.AreEqual(expectedMimeType, document.MimeType);
                pdfDocumentBytes.SequenceEqual(document.Bytes);
            }

            [TestMethod]
            public void InitializesAllValuesWithDocumentPath()
            {
                //Arrange
                const string subject = "subject";
                const string message = "message";
                const string fileName = "NavnPåFil";
                const FileType fileType = FileType.Txt;
                const string expectedMimeType = "text/plain";

                var documentPath = DocumentFilePath();

                //Act
                var document = new DocumentStub(
                    subject,
                    message,
                    fileName,
                    fileType,
                    documentPath
                    );

                var pdfDocumentBytes = File.ReadAllBytes(documentPath);

                //Assert
                Assert.AreEqual(subject, document.Title);
                Assert.AreEqual(message, document.Message);
                Assert.AreEqual(fileName, document.FileName);
                Assert.AreEqual(expectedMimeType, document.MimeType);
                pdfDocumentBytes.SequenceEqual(document.Bytes);
            }

            private static string DocumentFilePath()
            {
                var executingAssembly = Assembly.GetExecutingAssembly();
                var executablePath = Path.GetDirectoryName(executingAssembly.Location);
                var documentPath = Path.Combine(executablePath, "Resources", "Documents", "Dokument.pdf");
                return documentPath;
            }
        }

        [TestClass]
        public class IdMethod : DocumentTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticString()
            {
                //Arrange
                var id = "Id_0";

                //Act
                var document = CoreDomainUtility.GetDocument();

                //Assert
                Assert.AreEqual(id, document.Id);
            }
        }
    }
}