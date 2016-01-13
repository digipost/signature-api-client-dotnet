using System.Text;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice.AsiceManifest
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
                const string fileName = "manifest.xml";
                const string mimeType = "application/xml";
                var sender = DomainUtility.GetSender();
                var document = DomainUtility.GetDocument();
                var signers = DomainUtility.GetSigners(3);

                //Act
                var manifest = new Manifest(sender, document, signers);

                //Assert
                Assert.AreEqual(fileName, manifest.FileName);
                Assert.AreEqual(mimeType, manifest.MimeType);
                Assert.AreEqual(sender, manifest.Sender);
                Assert.AreEqual(document, manifest.Document);
                Assert.AreEqual(signers, manifest.Signers);
            }
        }

        [TestClass]
        public class BytesMethod : ManifestTests
        {
            [TestMethod]
            public void SuccessfulManifestToBytes()
            {
                //Arrange
                var manifest = DomainUtility.GetManifest();
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
