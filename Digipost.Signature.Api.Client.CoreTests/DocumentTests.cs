using System.IO;
using System.Linq;
using System.Reflection;
using Digipost.Signature.Api.Client.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.CoreTests
{
    [TestClass()]
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
                var pdfDocumentBytes = DomainUtility.GetPdfDocumentBytes();

                //Act
                var document = new Document(
                    subject, 
                    message, 
                    fileName, 
                    fileType, 
                    pdfDocumentBytes
                    );

                //Assert
                Assert.AreEqual(subject, document.Subject);
                Assert.AreEqual(message, document.Message);
                Assert.AreEqual(fileName, document.FileName);
                Assert.AreEqual(fileType, document.FileType);
                Assert.AreEqual(fileType, document.FileType);
            }
            
            [TestMethod]
            public void InitializesAllValuesWithDocumentPath()
            {
                //Arrange
                const string subject = "subject";
                const string message = "message";
                const string fileName = "NavnPåFil";
                const FileType fileType = FileType.Pdf;

                var documentPath = DocumentFilePath();

                //Act
                var document = new Document(
                    subject,
                    message,
                    fileName,
                    fileType,
                    documentPath
                    );

                var pdfDocumentBytes = File.ReadAllBytes(documentPath);

                //Assert
                Assert.AreEqual(subject, document.Subject);
                Assert.AreEqual(message, document.Message);
                Assert.AreEqual(fileName, document.FileName);
                Assert.AreEqual(fileType, document.FileType);
                Enumerable.SequenceEqual(pdfDocumentBytes, document.Bytes);
            }

            private static string DocumentFilePath()
            {
                var executingAssembly = Assembly.GetExecutingAssembly();
                var executablePath = Path.GetDirectoryName(executingAssembly.Location);
                var documentPath = Path.Combine(executablePath, "Resources", "Documents", "Dokument.pdf");
                return documentPath;
            }
        }
    }
}