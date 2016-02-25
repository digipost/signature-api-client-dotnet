using Difi.Felles.Utility;
using Difi.Felles.Utility.Utilities;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Internal
{
    [TestClass]
    public class CertificateValidatorTests
    {
        [TestClass]
        public class IsValidServerCertificateMethod : CertificateValidatorTests
        {
            [TestMethod]
            public void ReturnsFalseWithNullCertificate()
            {
                //Arrange
                var sertifikatkjedevalidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var clientConfiguration = CoreDomainUtility.GetClientConfiguration();

                //Act
                var isValid = CertificateValidator.IsValidServerCertificate(sertifikatkjedevalidator, null, clientConfiguration.ServerCertificateOrganizationNumber);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfNotIssuedToServerOrganizationNumber()
            {
                //Arrange
                var sertifikatkjedevalidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var clientConfiguration = CoreDomainUtility.GetClientConfiguration();

                //Act
                var isValid = CertificateValidator.IsValidServerCertificate(sertifikatkjedevalidator, CoreDomainUtility.GetTestIntegrasjonSertifikat(), clientConfiguration.ServerCertificateOrganizationNumber);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfNotActivated()
            {
                //Arrange
                var sertifikatkjedevalidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var clientConfiguration = CoreDomainUtility.GetClientConfiguration();

                //Act
                var isValid = CertificateValidator.IsValidServerCertificate(sertifikatkjedevalidator, CoreDomainUtility.GetNotActivatedSelfSignedCertificate(), clientConfiguration.ServerCertificateOrganizationNumber);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfExpired()
            {
                //Arrange
                var sertifikatkjedevalidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var clientConfiguration = CoreDomainUtility.GetClientConfiguration();

                //Act
                var isValid = CertificateValidator.IsValidServerCertificate(sertifikatkjedevalidator, CoreDomainUtility.GetExpiredSelfSignedCertificate(), clientConfiguration.ServerCertificateOrganizationNumber);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsTrueForCorrectCertificate()
            {
                //Arrange
                var sertifikatkjedevalidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var clientConfiguration = CoreDomainUtility.GetClientConfiguration();

                //Act
                var isValid = CertificateValidator.IsValidServerCertificate(sertifikatkjedevalidator, CoreDomainUtility.GetPostenTestCertificate(), clientConfiguration.ServerCertificateOrganizationNumber);

                //Assert
                Assert.IsTrue(isValid);
            }
        }
    }
}