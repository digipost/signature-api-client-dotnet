using Digipost.Signature.Api.Client.Asice;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.AsiceTests.AsiceManifest
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
                byte[] bytes = {0x20, 0x21, 0x22};
                const string fileName = "manifest.xml";
                const string mimeType = "application/xml";

                //Act
                var manifest = new Manifest(bytes);

                //Assert
                CollectionAssert.AreEqual(bytes, manifest.Bytes);
                Assert.AreEqual(fileName, manifest.FileName);
                Assert.AreEqual(mimeType, manifest.MimeType);
            }
        }
    }
}
