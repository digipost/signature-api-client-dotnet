using System.Text;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests.Internal.AsicE
{
    [TestClass]
    public class PortalManifestTests
    {
        [TestClass]
        public class ConstructorMethod : PortalManifestTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var sender = DomainUtility.GetSender();
                var document = DomainUtility.GetDocument();
                var signers = DomainUtility.GetSigners(2);

                //Act
                var manifest = new PortalManifest(sender, document, signers);

                //Assert
                Assert.AreEqual(sender, manifest.Sender);
                Assert.AreEqual(document, manifest.Document);
                Assert.AreEqual(signers, manifest.Signers);
            }
        }

        [TestClass]
        public class FileNameMethod : PortalManifestTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticString()
            {
                //Arrange
                const string fileName = "manifest.xml";
                var manifest = DomainUtility.GetPortalManifest();

                //Act

                //Assert
                Assert.AreEqual(fileName, manifest.FileName);
            }
        }

        [TestClass]
        public class MimeTypeMethod : PortalManifestTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticString()
            {
                //Arrange
                const string mimeType = "application/xml";
                var manifest = DomainUtility.GetPortalManifest();

                //Act

                //Assert
                Assert.AreEqual(mimeType, manifest.MimeType);
            }
        }

        [TestClass]
        public class IdMethod : PortalManifestTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticString()
            {
                //Arrange
                const string id = "Id_1";
                var manifest = DomainUtility.GetPortalManifest();

                //Act

                //Assert
                Assert.AreEqual(id, manifest.Id);
            }
        }

        [TestClass]
        public class BytesMethod : PortalManifestTests
        {
            [TestMethod]
            public void SuccessfulManifestToBytes()
            {
                //Arrange
                var manifest = DomainUtility.GetPortalManifest();
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