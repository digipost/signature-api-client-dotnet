using System.Text;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests.Internal.AsicE
{
    public class ManifestTests
    {
        public class ConstructorMethod : ManifestTests
        {
            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                var sender = CoreDomainUtility.GetSender();
                var documents = DomainUtility.GetSingleDirectDocument();
                var signer = DomainUtility.GetSigner();

                //Act
                var manifest = new Manifest("Job title", sender, documents, signer);

                //Assert
                Assert.Equal(sender, manifest.Sender);
                Assert.Equal(documents, manifest.Documents);
                Assert.Equal(signer, manifest.Signers);
            }
        }

        public class FileNameMethod : ManifestTests
        {
            [Fact]
            public void ReturnsCorrectStaticString()
            {
                //Arrange
                const string fileName = "manifest.xml";
                var manifest = DomainUtility.GetDirectManifest();

                //Act

                //Assert
                Assert.Equal(fileName, manifest.FileName);
            }
        }

        public class MimeTypeMethod : ManifestTests
        {
            [Fact]
            public void ReturnsCorrectStaticString()
            {
                //Arrange
                const string mimeType = "application/xml";
                var manifest = DomainUtility.GetDirectManifest();

                //Act

                //Assert
                Assert.Equal(mimeType, manifest.MimeType);
            }
        }

        public class IdMethod : ManifestTests
        {
            [Fact]
            public void ReturnsCorrectStaticString()
            {
                //Arrange
                const string id = "Id_1";
                var manifest = DomainUtility.GetDirectManifest();

                //Act

                //Assert
                Assert.Equal(id, manifest.Id);
            }
        }

        public class BytesMethod : ManifestTests
        {
            [Fact]
            public void SuccessfulManifestToBytes()
            {
                //Arrange
                var manifest = DomainUtility.GetDirectManifest();
                var manifestDataTranferObject = DataTransferObjectConverter.ToDataTransferObject(manifest);
                var expectedResult = SerializeUtility.Serialize(manifestDataTranferObject);

                //Act
                var bytes = manifest.Bytes;
                var actualResult = Encoding.UTF8.GetString(bytes);

                //Assert
                Assert.Equal(expectedResult, actualResult);
            }
        }
    }
}
