using System.Net;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class BaseClientTests
    {
        [TestClass]
        public class ConstructorMethod : BaseClientTests
        {
            [TestMethod]
            public void InitializesWithProperties()
            {
                //Arrange
                var tlsSetup = SecurityProtocolType.Tls12;
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetTestIntegrasjonSertifikat())
                {
                    HttpClientTimeoutInMilliseconds = 1441
                };

                //Act
                var clientStub = new ClientStub(clientConfiguration);

                //Assert
                Assert.AreEqual(clientConfiguration, clientStub.ClientConfiguration);
                Assert.AreEqual(tlsSetup, ServicePointManager.SecurityProtocol);
                Assert.IsNotNull(clientStub.RequestHelper);
                Assert.AreEqual(clientConfiguration.HttpClientTimeoutInMilliseconds, clientStub.HttpClient.Timeout.TotalMilliseconds);
            }
        }

        [TestClass]
        public class CurrentSenderMethod : BaseClientTests
        {
            [TestMethod]
            [ExpectedException(typeof(SenderNotSpecifiedException))]
            public void ThrowsExceptionOnNoSender()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate());
                var client = new ClientStub(clientConfiguration);

                //Act
                client.GetCurrentSender(null);

                //Assert
                Assert.Fail();
            }

            [TestMethod]
            public void ReturnsClientClientConfigurationSenderIfOnlySet()
            {
                //Arrange
                var expected = new Sender("000000000");
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate(), expected);
                var client = new ClientStub(clientConfiguration);

                //Act
                var actual = client.GetCurrentSender(null);

                //Assert
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void ReturnsJobSenderIfOnlySet()
            {
                //Arrange
                var expected = new Sender("000000000");
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate());
                var client = new ClientStub(clientConfiguration);

                //Act
                var actual = client.GetCurrentSender(expected);

                //Assert
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void ReturnsJobSenderIfBothSet()
            {
                //Arrange
                var expected = new Sender("000000000");
                var clientConfigurationSender = new Sender("999999999");
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate(), clientConfigurationSender);
                var client = new ClientStub(clientConfiguration);

                //Act
                var actual = client.GetCurrentSender(expected);

                //Assert
                Assert.AreEqual(expected, actual);
            }
        }
    }
}