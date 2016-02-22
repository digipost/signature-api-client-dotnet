using Digipost.Signature.Api.Client.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Extensions
{
    [TestClass]
    public class FileTypeExtensionsTests
    {
        [TestClass]
        public class ToMimeTypeMethod : FileTypeExtensionsTests
        {
            [TestMethod]
            public void ConvertsPdf()
            {
                //Arrange
                const string expectedPdfMimeType = "application/pdf";

                //Act
                var pdfMimeType = FileType.Pdf.ToMimeType();

                //Assert
                Assert.AreEqual(expectedPdfMimeType, pdfMimeType);
            }

            [TestMethod]
            public void ConvertsTxt()
            {
                //Arrange
                const string expectedTxtMimeType = "text/plain";

                //Act
                var txtMimeType = FileType.Txt.ToMimeType();

                //Assert
                Assert.AreEqual(expectedTxtMimeType, txtMimeType);
            }

        }
    }
}
