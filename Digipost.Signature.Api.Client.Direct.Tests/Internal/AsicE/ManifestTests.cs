using System.Text;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Internal.AsicE
{
    public class ManifestTests
    {
        [TestClass]
        public class ConstructorMethod : ManifestTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var sender = CoreDomainUtility.GetSender();
                var document = DomainUtility.GetDirectDocument();
                var signer = DomainUtility.GetSigner();

                //Act
                var manifest = new Manifest(sender, document, signer);

                //Assert
                Assert.AreEqual(sender, manifest.Sender);
                Assert.AreEqual(document, manifest.Document);
                Assert.AreEqual(signer, manifest.Signer);
            }
        }

        [TestClass]
        public class FileNameMethod : ManifestTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticString()
            {
                //Arrange
                const string fileName = "manifest.xml";
                var manifest = DomainUtility.GetDirectManifest();

                //Act

                //Assert
                Assert.AreEqual(fileName, manifest.FileName);
            }
        }

        [TestClass]
        public class MimeTypeMethod : ManifestTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticString()
            {
                //Arrange
                const string mimeType = "application/xml";
                var manifest = DomainUtility.GetDirectManifest();

                //Act

                //Assert
                Assert.AreEqual(mimeType, manifest.MimeType);
            }
        }

        [TestClass]
        public class IdMethod : ManifestTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticString()
            {
                //Arrange
                const string id = "Id_1";
                var manifest = DomainUtility.GetDirectManifest();

                //Act

                //Assert
                Assert.AreEqual(id, manifest.Id);
            }
        }

        [TestClass]
        public class BytesMethod : ManifestTests
        {
            [TestMethod]
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
                Assert.AreEqual(expectedResult, actualResult);
            }
        }
    }
}