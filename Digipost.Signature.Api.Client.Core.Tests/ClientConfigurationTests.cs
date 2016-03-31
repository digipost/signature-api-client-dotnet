using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core.Asice;
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
                new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate());

                //Assert
            }

            [TestMethod]
            public void ConstructorWithCertificateThumbprint()
            {
                //Arrange
                var environment = Environment.DifiQa;
                var sender = CoreDomainUtility.GetSender();

                var aCertificateFromCertificateStore = CoreDomainUtility.GetPostenTestCertificate();

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
        }

        [TestClass]
        public class EnableDocumentBundleDiskDumpMethod : ClientConfigurationTests
        {
            [TestMethod]
            public void AddsDocumentBundleToDiskProcessor()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate());

                //Act
                clientConfiguration.EnableDocumentBundleDiskDump(@"\\vmware-host\Shared Folders\Downloads");

                //Assert
                Assert.IsTrue(clientConfiguration.DocumentBundleProcessors.Any(p => p.GetType() == typeof (DocumentBundleToDiskProcessor)));
            }
        }
    }
}