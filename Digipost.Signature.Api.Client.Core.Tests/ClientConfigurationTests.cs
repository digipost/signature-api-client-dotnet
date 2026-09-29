using System;
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
            [Fact(Skip = "Skipping - does not run on Linux yet.")]
            public void Constructor_with_certificate_thumbprint()
            {
                //Arrange
                var environment = Environment.DifiTest;
                var sender = CoreDomainUtility.GetSender();

                var certificate = CertificateUtility.SenderCertificate("2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed");

                //Act
                var clientConfiguration = new ClientConfiguration(
                    environment,
                    certificate.Thumbprint, CoreDomainUtility.JwtClientId, CoreDomainUtility.GetAccountId(), sender);

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
                new ClientConfiguration(Environment.DifiTest, CoreDomainUtility.GetPostenTestCertificate(), CoreDomainUtility.JwtClientId, CoreDomainUtility.GetAccountId());

                //Assert
            }

            [Fact]
            public void Constructor_with_client_id_and_account_id()
            {
                //Arrange
                var environment = Environment.DifiTest;
                var sender = CoreDomainUtility.GetSender();
                var x509Certificate = CoreDomainUtility.GetTestCertificate();
                var accountId = new AccountId("123456");

                //Act
                var clientConfiguration = new ClientConfiguration(
                    environment,
                    x509Certificate,
                    "client-id",
                    accountId,
                    sender);

                //Assert
                Assert.Equal(environment, clientConfiguration.Environment);
                Assert.Equal(sender, clientConfiguration.GlobalSender);
                Assert.Equal(x509Certificate, clientConfiguration.Certificate);
                Assert.Equal("client-id", clientConfiguration.ClientId);
                Assert.Equal(accountId, clientConfiguration.AccountId);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("   ")]
            public void Throws_on_blank_client_id(string clientId)
            {
                //Arrange
                //Act
                //Assert
                Assert.Throws<ArgumentException>(() => new ClientConfiguration(
                    Environment.DifiTest,
                    CoreDomainUtility.GetTestCertificate(),
                    clientId,
                    new AccountId("123456")));
            }

            [Fact]
            public void Throws_on_null_account_id()
            {
                //Arrange
                //Act
                //Assert
                Assert.Throws<ArgumentNullException>(() => new ClientConfiguration(
                    Environment.DifiTest,
                    CoreDomainUtility.GetTestCertificate(),
                    "client-id",
                    null));
            }
        }

        public class EnableDocumentBundleDiskDumpMethod : ClientConfigurationTests
        {
            [Fact]
            public void Adds_document_bundle_to_disk_processor()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiTest, CoreDomainUtility.GetPostenTestCertificate(), CoreDomainUtility.JwtClientId, CoreDomainUtility.GetAccountId());

                //Act
                clientConfiguration.EnableDocumentBundleDiskDump(@"\\vmware-host\Shared Folders\Downloads");

                //Assert
                Assert.Contains(clientConfiguration.DocumentBundleProcessors, p => p.GetType() == typeof(DocumentBundleToDiskProcessor));
            }
        }
    }
}
