using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice.AsiceSignature
{
    [TestClass]
    public class SignatureGeneratorTests
    {
        [TestClass]
        public class ConstructorMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void InitializesWithDocumentManifestAndCertificate()
            {
                //Arrange
                var document = DomainUtility.GetDocument();
                var sender = DomainUtility.GetSender();
                var manifest = new Manifest(sender, document, DomainUtility.GetSigners(3));
                var x509Certificate2 = DomainUtility.GetCertificate();

                //Act
                SignaturGenerator signaturGenerator = new SignaturGenerator(document, manifest, x509Certificate2);

                //Assert
                Assert.AreEqual(document, signaturGenerator.Document);
                Assert.AreEqual(manifest, signaturGenerator.Manifest);
                Assert.AreEqual(x509Certificate2, signaturGenerator.Certificate);
            }
        }

        [TestClass]
        public class FileNameMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticFileName()
            {
                //Arrange
                var signaturGenerator = GetSignaturGenerator();
                var expectedFileName = "META-INF/signatures.xml";

                //Act

                //Assert
                Assert.AreEqual(expectedFileName, signaturGenerator.FileName);
            }
        }

        [TestClass]
        public class IdMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticId()
            {
                //Arrange
                var signaturGenerator = GetSignaturGenerator();
                var expectedId = "Id_0";

                //Act

                //Assert
                Assert.AreEqual(expectedId, signaturGenerator.Id);
            }
        }

        [TestClass]
        public class XmlMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void GeneratesCorrectXml()
            {
                //Arrange
                

                //Act

                //Assert
                Assert.Fail();
            }

        }

        internal SignaturGenerator GetSignaturGenerator()
        {
            var document = DomainUtility.GetDocument();
            var sender = DomainUtility.GetSender();
            var manifest = new Manifest(sender, document, DomainUtility.GetSigners(3));
            var x509Certificate2 = DomainUtility.GetCertificate();
            var signaturGenerator = new SignaturGenerator(document, manifest, x509Certificate2);
            return signaturGenerator;
        }
    }
}