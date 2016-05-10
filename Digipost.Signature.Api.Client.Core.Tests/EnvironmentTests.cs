using System;
using Difi.Felles.Utility.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class EnvironmentTests
    {
        [TestClass]
        public class GetEnvironmentMethod : EnvironmentTests
        {
            [TestMethod]
            public void GetsInitializeLocalhostEnvironment()
            {
                //Arrange
                var url = new Uri("https://172.16.91.1:8443");
                var certificates = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act
                var environment = Environment.Localhost;

                //Assert
                Assert.IsNotNull(environment.CertificateChainValidator);
                Assert.AreEqual(url, environment.Url);
                CollectionAssert.AreEqual(certificates, environment.CertificateChainValidator.SertifikatLager);
            }

            [TestMethod]
            public void GetsInitializedDifiTestEnvironment()
            {
                //Arrange
                var url = new Uri("https://api.difitest.signering.posten.no");
                var certificates = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act
                var environment = Environment.DifiTest;

                //Assert
                Assert.IsNotNull(environment.CertificateChainValidator);
                Assert.AreEqual(url, environment.Url);
                CollectionAssert.AreEqual(certificates, environment.CertificateChainValidator.SertifikatLager);
            }

            [TestMethod]
            public void GetsInitializedDifiQaEnvironment()
            {
                //Arrange
                var url = new Uri("https://api.difiqa.signering.posten.no");
                var certificates = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act
                var environment = Environment.DifiQa;

                //Assert
                Assert.IsNotNull(environment.CertificateChainValidator);
                Assert.AreEqual(url, environment.Url);
                CollectionAssert.AreEqual(certificates, environment.CertificateChainValidator.SertifikatLager);
            }

            [TestMethod]
            public void GetsInitializedProductionEnvironment()
            {
                //Arrange
                var url = new Uri("https://api.signering.posten.no");
                var certificates = CertificateChainUtility.ProduksjonsSertifikater();

                //Act
                var environment = Environment.Production;

                //Assert
                Assert.IsNotNull(environment.CertificateChainValidator);
                Assert.AreEqual(url, environment.Url);
                CollectionAssert.AreEqual(certificates, environment.CertificateChainValidator.SertifikatLager);
            }

        }
    }
}