using System.Net.Security;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class DirectClientTests
    {
        [TestClass]
        public class ConstructorMethod : DirectClientTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var clientConfiguration = CoreDomainUtility.GetClientConfiguration();

                //Act
                var client = new DirectClient(clientConfiguration);

                //Assert
                Assert.AreEqual(clientConfiguration, client.ClientConfiguration);
                Assert.IsNotNull(client.HttpClient);
                Assert.IsNotNull(client.HttpClient);
            }
        }

        [TestClass]
        public class IsValidServerCertificateMethod : DirectClientTests
        {
            [TestMethod]
            public void ReturnsFalseWithNullCertificate()
            {
                //Arrange
                var directClient = new DirectClient(CoreDomainUtility.GetClientConfiguration());

                //Act
                var isValid = directClient.IsValidServerCertificate(null, null, null, SslPolicyErrors.None);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfNotIssuedToServerOrganizationNumber()
            {
                //Arrange
                var directClient = new DirectClient(CoreDomainUtility.GetClientConfiguration());

                //Act
                var isValid = directClient.IsValidServerCertificate(null, CoreDomainUtility.GetTestIntegrasjonSertifikat(), null, SslPolicyErrors.None);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfExpired()
            {
                //Arrange
                var directClient = new DirectClient(CoreDomainUtility.GetClientConfiguration());

                //Act
                var isValid = directClient.IsValidServerCertificate(null, CoreDomainUtility.GetExpiredSelfSignedCertificate(), null, SslPolicyErrors.None);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfNotActivated()
            {
                //Arrange
                var directClient = new DirectClient(CoreDomainUtility.GetClientConfiguration());

                //Act
                var isValid = directClient.IsValidServerCertificate(null, CoreDomainUtility.GetNotActivatedSelfSignedCertificate(), null, SslPolicyErrors.None);

                //Assert
                Assert.IsFalse(isValid);
            }



        }
    }
}