using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class AbstractDocumentTests
    {
        [TestClass]
        public class ConstructorMethod : AbstractDocumentTests
        {
            [TestMethod]
            public void InitializesAllValuesWithDocumentBytes()
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
                Assert.AreEqual(subject, document.Title);
                Assert.AreEqual(message, document.Message);
                Assert.AreEqual(expectedMimeType, document.MimeType);
                pdfDocumentBytes.SequenceEqual(document.Bytes);
            }

            [TestMethod]
            public void InitializesAllValuesWithDocumentPath()
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
                Assert.AreEqual(subject, document.Title);
                Assert.AreEqual(message, document.Message);
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
        public class IdMethod : AbstractDocumentTests
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

        [TestClass]
        public class FileNameMethod : AbstractDocumentTests
        {
            [TestMethod]
            public void ReturnsFileNameWithDate()
            {
                //Arrange

                //Act
                var document = new DocumentStub("title", "message", FileType.Txt, new byte[] {0xb});

                //Assert
                Assert.IsTrue(document.FileName.Contains(DateTime.Now.ToString("yyyyMMdd")) && document.FileName.Contains("document"));
            }
        }
    }
}