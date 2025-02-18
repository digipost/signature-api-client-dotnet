using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class ClientConfigurationTests
    {
        public class ConstructorMethod : ClientConfigurationTests
        {
            [Fact]
            public void Constructor_with_certificate()
            {
                //Arrange
                var environment = Environment.DifiQa;
                var sender = CoreDomainUtility.GetSender();
                var x509Certificate = CoreDomainUtility.GetTestCertificate();

                //Act
                var clientConfiguration = new ClientConfiguration(
                    environment,
                    x509Certificate, sender);

                //Assert
                Assert.Equal(environment, clientConfiguration.Environment);
                Assert.Equal(sender, clientConfiguration.GlobalSender);
                Assert.Equal(x509Certificate, clientConfiguration.Certificate);
            }

            [Fact(Skip = "Skipping - does not run on Linux yet.")]
            public void Constructor_with_certificate_thumbprint()
            {
                //Arrange
                var environment = Environment.DifiQa;
                var sender = CoreDomainUtility.GetSender();

                var certificate = CertificateUtility.SenderCertificate("2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed");

                //Act
                var clientConfiguration = new ClientConfiguration(
                    environment,
                    certificate.Thumbprint, sender);

                //Assert
                Assert.Equal(environment, clientConfiguration.Environment);
                Assert.Equal(sender, clientConfiguration.GlobalSender);
                Assert.Equal(certificate, clientConfiguration.Certificate);
            }

            [Fact]
            public void Constructor_with_no_sender_and_certificate_thumbprint_exists()
            {
                //Arrange

                //Act
                new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate());

                //Assert
            }
        }

        public class EnableDocumentBundleDiskDumpMethod : ClientConfigurationTests
        {
            [Fact]
            public void Adds_document_bundle_to_disk_processor()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate());

                //Act
                clientConfiguration.EnableDocumentBundleDiskDump(@"\\vmware-host\Shared Folders\Downloads");

                //Assert
                Assert.Contains(clientConfiguration.DocumentBundleProcessors, p => p.GetType() == typeof(DocumentBundleToDiskProcessor));
            }
        }
    }
}
