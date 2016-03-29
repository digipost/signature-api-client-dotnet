using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class ClientConfigurationTests
    {
        [TestClass]
        public class ConstructorMethod : ClientConfigurationTests
        {
            [TestMethod]
            public void ConstructorWithNoSenderAndCertificateThumbprintExists()
            {
                //Arrange

                //Act
                new ClientConfiguration(Environment.DifiQa, GetFirstCertificateFromCurrentUserStore());

                //Assert
            }

            [TestMethod]
            public void ConstructorWithCertificateThumbprint()
            {
                //Arrange
                var environment = Environment.DifiQa;
                var sender = CoreDomainUtility.GetSender();

                var aCertificateFromCertificateStore = GetFirstCertificateFromCurrentUserStore();

                //Act
                var clientConfiguration = new ClientConfiguration(
                    environment,
                    aCertificateFromCertificateStore.Thumbprint, sender);

                //Assert
                Assert.AreEqual(environment, clientConfiguration.Environment);
                Assert.AreEqual(sender, clientConfiguration.GlobalSender);
                Assert.AreEqual(aCertificateFromCertificateStore, clientConfiguration.Certificate);
            }

            [TestMethod]
            public void ConstructorWithCertificate()
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
                Assert.AreEqual(environment, clientConfiguration.Environment);
                Assert.AreEqual(sender, clientConfiguration.GlobalSender);
                Assert.AreEqual(x509Certificate, clientConfiguration.Certificate);
            }

            [TestMethod]
            public void ConstructorWithNoSenderAndCertificateExists()
            {
                //Arrange

                //Act
                new ClientConfiguration(Environment.DifiQa, new X509Certificate2());

                //Assert
            }

            private static X509Certificate2 GetFirstCertificateFromCurrentUserStore()
            {
                var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

                store.Open(OpenFlags.ReadOnly);
                var certificate = store.Certificates[0];
                store.Close();

                return certificate;
            }
        }
    }
}