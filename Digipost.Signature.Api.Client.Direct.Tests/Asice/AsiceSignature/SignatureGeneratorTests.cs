using System.Linq;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Internal.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests.Asice.AsiceSignature
{
    public class SignatureGeneratorTests
    {
        internal SignatureGenerator GetSignaturGenerator()
        {
            var document = DomainUtility.GetDirectDocument();
            var sender = CoreDomainUtility.GetSender();
            var signers = DomainUtility.GetSigner();
            var manifest = new Manifest(sender, document, signers);
            var x509Certificate2 = CoreDomainUtility.GetTestCertificate();
            var signaturGenerator = new SignatureGenerator(x509Certificate2, document, manifest);
            return signaturGenerator;
        }

        public class ConstructorMethod : SignatureGeneratorTests
        {
            [Fact]
            public void InitializesWithDocumentDirectManifestAndCertificate()
            {
                //Arrange
                var document = DomainUtility.GetDirectDocument();
                var sender = CoreDomainUtility.GetSender();
                var manifest = new Manifest(sender, document, DomainUtility.GetSigner());
                var x509Certificate2 = CoreDomainUtility.GetTestCertificate();

                //Act
                var signatur = new SignatureGenerator(x509Certificate2, document, manifest);

                //Assert
                Assert.Equal(document, signatur.Attachables.ElementAt(0));
                Assert.Equal(manifest, signatur.Attachables.ElementAt(1));
                Assert.Equal(x509Certificate2, signatur.Certificate);
            }
        }

        public class FileNameMethod : SignatureGeneratorTests
        {
            [Fact]
            public void ReturnsCorrectStaticFileName()
            {
                //Arrange
                var signaturGenerator = GetSignaturGenerator();
                const string expectedFileName = "META-INF/signatures.xml";

                //Act

                //Assert
                Assert.Equal(expectedFileName, signaturGenerator.FileName);
            }
        }

        public class IdMethod : SignatureGeneratorTests
        {
            [Fact]
            public void ReturnsCorrectStaticId()
            {
                //Arrange
                var signaturGenerator = GetSignaturGenerator();
                const string expectedId = "Id_0";

                //Act

                //Assert
                Assert.Equal(expectedId, signaturGenerator.Id);
            }
        }

        public class XmlMethod : SignatureGeneratorTests
        {
            [Fact]
            public void GeneratesValidSignatureXml()
            {
                //Arrange
                var signatureGenerator = DomainUtility.GetSignature();
                var xml = signatureGenerator.Xml().InnerXml;
                var signatureValidator = new SignatureValidator();

                //Act
                var isValidSignatureXml = signatureValidator.Validate(xml);
                var signatureLength = xml.Length;

                //Assert
                Assert.True(isValidSignatureXml);
                Assert.True(signatureLength > 3200);
            }
        }
    }
}