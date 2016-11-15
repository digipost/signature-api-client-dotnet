using Digipost.Signature.Api.Client.Core.Internal.Extensions;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Extensions
{
    public class FileTypeExtensionsTests
    {
        public class ToMimeTypeMethod : FileTypeExtensionsTests
        {
            [Fact]
            public void Converts_pdf()
            {
                //Arrange
                const string expectedPdfMimeType = "application/pdf";

                //Act
                var pdfMimeType = FileType.Pdf.ToMimeType();

                //Assert
                Assert.Equal(expectedPdfMimeType, pdfMimeType);
            }

            [Fact]
            public void Converts_txt()
            {
                //Arrange
                const string expectedTxtMimeType = "text/plain";

                //Act
                var txtMimeType = FileType.Txt.ToMimeType();

                //Assert
                Assert.Equal(expectedTxtMimeType, txtMimeType);
            }
        }
    }
}