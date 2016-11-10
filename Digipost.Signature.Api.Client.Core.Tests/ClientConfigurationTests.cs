using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
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

                var certificate = CertificateUtility.SenderCertificate("2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed", Language.English);

                //Act
                var clientConfiguration = new ClientConfiguration(
                    environment,
                    certificate.Thumbprint, sender);

                //Assert
                Assert.AreEqual(environment, clientConfiguration.Environment);
                Assert.AreEqual(sender, clientConfiguration.GlobalSender);
                Assert.AreEqual(certificate, clientConfiguration.Certificate);
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
                Assert.IsTrue(clientConfiguration.DocumentBundleProcessors.Any(p => p.GetType() == typeof(DocumentBundleToDiskProcessor)));
            }
        }
    }
}