using System.Linq;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Internal.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests.Asice.AsiceSignature
{
    public class SignatureGeneratorTests
    {
        internal SignatureGenerator GetSignaturGenerator()
        {
            var documents = DomainUtility.GetPortalDocuments();
            var sender = CoreDomainUtility.GetSender();
            var signers = DomainUtility.GetSigners(2);
            var manifest = new Manifest("JobTitle", sender, documents, signers);
            var x509Certificate2 = CoreDomainUtility.GetTestCertificate();
            var signaturGenerator = new SignatureGenerator(x509Certificate2, documents, manifest);
            return signaturGenerator;
        }

        public class ConstructorMethod : SignatureGeneratorTests
        {
            [Fact]
            public void Initializes_with_document_portal_manifest_and_certificate()
            {
                //Arrange
                var documents = DomainUtility.GetPortalDocuments();
                var sender = CoreDomainUtility.GetSender();
                var manifest = new Manifest("JobTitle", sender, documents, DomainUtility.GetSigners(3));
                var x509Certificate2 = CoreDomainUtility.GetTestCertificate();

                //Act
                var signatur = new SignatureGenerator(x509Certificate2, documents, manifest);

                //Assert
                Assert.Equal(documents.ElementAt(0), signatur.Attachables.ElementAt(0));
                Assert.Equal(manifest, signatur.Attachables.ElementAt(1));
                Assert.Equal(x509Certificate2, signatur.Certificate);
            }
        }

        public class FileNameMethod : SignatureGeneratorTests
        {
            [Fact]
            public void Returns_correct_static_file_name()
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
            public void Returns_correct_static_id()
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
            public void Generates_valid_signature_xml()
            {
                ////Arrange
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
