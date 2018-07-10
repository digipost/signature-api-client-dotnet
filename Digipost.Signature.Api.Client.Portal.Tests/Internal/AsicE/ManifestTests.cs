using System.Text;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests.Internal.AsicE
{
    public class ManifestTests
    {
        public class ConstructorMethod : ManifestTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var sender = CoreDomainUtility.GetSender();
                var document = DomainUtility.GetPortalDocument();
                var signers = DomainUtility.GetSigners(2);

                //Act
                var manifest = new Manifest(sender, document, signers);

                //Assert
                Assert.Equal(sender, manifest.Sender);
                Assert.Equal(document, manifest.Document);
                Assert.Equal(signers, manifest.Signers);
            }
        }

        public class FileNameMethod : ManifestTests
        {
            [Fact]
            public void Returns_correct_static_string()
            {
                //Arrange
                const string fileName = "manifest.xml";
                var manifest = DomainUtility.GetPortalManifest();

                //Act

                //Assert
                Assert.Equal(fileName, manifest.FileName);
            }
        }

        public class MimeTypeMethod : ManifestTests
        {
            [Fact]
            public void Returns_correct_static_string()
            {
                //Arrange
                const string mimeType = "application/xml";
                var manifest = DomainUtility.GetPortalManifest();

                //Act

                //Assert
                Assert.Equal(mimeType, manifest.MimeType);
            }
        }

        public class IdMethod : ManifestTests
        {
            [Fact]
            public void Returns_correct_static_string()
            {
                //Arrange
                const string id = "Id_1";
                var manifest = DomainUtility.GetPortalManifest();

                //Act

                //Assert
                Assert.Equal(id, manifest.Id);
            }
        }

        public class BytesMethod : ManifestTests
        {
            [Fact]
            public void Successful_manifest_to_bytes()
            {
                //Arrange
                var manifest = DomainUtility.GetPortalManifest();
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