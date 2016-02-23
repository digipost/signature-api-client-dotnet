using System;
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
            public void ConstructorWithCertificate()
            {
                //Arrange
                var signatureServiceRoot = new Uri("http://SignatureServiceRoot.no");
                var sender = CoreDomainUtility.GetSender();
                var x509Certificate = CoreDomainUtility.GetTestCertificate();

                //Act
                var clientConfiguration = new ClientConfiguration(
                    signatureServiceRoot,
                    sender,
                    x509Certificate
                    );

                //Assert
                Assert.AreEqual(signatureServiceRoot, clientConfiguration.SignatureServiceRoot);
                Assert.AreEqual(sender, clientConfiguration.Sender);
                Assert.AreEqual(x509Certificate, clientConfiguration.Certificate);
            }

            [TestMethod]
            public void ConstructorWithCertificateThumbprint()
            {
                //Arrange
                var signatureServiceRoot = new Uri("http://SignatureServiceRoot.no");
                var sender = CoreDomainUtility.GetSender();

                var aCertificateFromCertificateStore = GetFirstCertificateFromCurrentUserStore();

                //Act
                var clientConfiguration = new ClientConfiguration(
                    signatureServiceRoot,
                    sender,
                    aCertificateFromCertificateStore.Thumbprint
                    );

                //Assert
                Assert.AreEqual(signatureServiceRoot, clientConfiguration.SignatureServiceRoot);
                Assert.AreEqual(sender, clientConfiguration.Sender);
                Assert.AreEqual(aCertificateFromCertificateStore, clientConfiguration.Certificate);
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