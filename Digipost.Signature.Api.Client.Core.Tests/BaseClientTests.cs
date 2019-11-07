using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Xunit;
using static Digipost.Signature.Api.Client.Core.Tests.Utilities.CoreDomainUtility;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class BaseClientTests
    {
        public class ConstructorMethod : BaseClientTests
        {
            [Fact]
            public void Initializes_with_properties()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, GetBringCertificate())
                {
                    HttpClientTimeoutInMilliseconds = 1441
                };

                //Act
                var clientStub = new ClientStub(clientConfiguration);

                //Assert
                Assert.Equal(clientConfiguration, clientStub.ClientConfiguration);
                Assert.NotNull(clientStub.RequestHelper);
                Assert.Equal(clientConfiguration.HttpClientTimeoutInMilliseconds, clientStub.HttpClient.Timeout.TotalMilliseconds);
            }
        }

        public class CurrentSenderMethod : BaseClientTests
        {
            [Fact]
            public void Can_disable_sender_certificate_validation()
            {
                //Arrange
                var sender = new Sender(BringPublicOrganizationNumber);
                var incorrectSenderCertificate = GetPostenTestCertificate();
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, incorrectSenderCertificate)
                {
                    CertificateValidationPreferences = {ValidateSenderCertificate = false}
                };
                var client = new ClientStub(clientConfiguration);

                //Act
                var actual = client.GetCurrentSender(sender);

                //Assert
            }

            [Fact]
            public void Returns_client_client_configuration_sender_if_only_set()
            {
                //Arrange
                var expected = new Sender(BringPublicOrganizationNumber);
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, GetBringCertificate(), expected);
                var client = new ClientStub(clientConfiguration);

                //Act
                var actual = client.GetCurrentSender(null);

                //Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void Returns_job_sender_if_both_set()
            {
                //Arrange
                var expected = new Sender(BringPublicOrganizationNumber);
                var clientConfigurationSender = new Sender(PostenOrganizationNumber);
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, GetBringCertificate(), clientConfigurationSender);
                var client = new ClientStub(clientConfiguration);

                //Act
                var actual = client.GetCurrentSender(expected);

                //Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void Returns_job_sender_if_only_set()
            {
                //Arrange
                var expected = new Sender(BringPublicOrganizationNumber);
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, GetBringCertificate());
                var client = new ClientStub(clientConfiguration);

                //Act
                var actual = client.GetCurrentSender(expected);

                //Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void Throws_exception_on_no_sender()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, GetPostenTestCertificate());
                var client = new ClientStub(clientConfiguration);

                //Act
                Assert.Throws<SenderNotSpecifiedException>(() => client.GetCurrentSender(null));
            }
        }
    }
}
