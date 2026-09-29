using System;
using Digipost.Api.Client.Shared.Certificate;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class EnvironmentTests
    {
        public class GetEnvironmentMethod : EnvironmentTests
        {
            [Fact]
            public void Gets_initialize_localhost_environment()
            {
                //Arrange
                var url = new Uri("https://localhost:8443");
                var certificates = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act
                var environment = Environment.Localhost;

                //Assert
                Assert.Equal(url, environment.Url);
                Assert.Equal(certificates, environment.AllowedChainCertificates);
            }
            
            [Fact]
            public void Gets_initialize_docker_localhost_environment()
            {
                //Arrange
                var url = new Uri("https://host.docker.internal:8443");
                var certificates = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act
                var environment = Environment.DockerLocalhost;

                //Assert
                Assert.Equal(url, environment.Url);
                Assert.Equal(certificates, environment.AllowedChainCertificates);
            }

            [Fact]
            public void Gets_initialized_difi_qa_environment()
            {
                //Arrange
                var url = new Uri("https://api.difiqa.signering.posten.no");
                var certificates = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();
                var tokenEndpoint = new Uri("https://midp.qa.digipost.no/oauth2/token");

                //Act
                var environment = Environment.DifiQa;

                //Assert
                Assert.Equal(url, environment.Url);
                Assert.Equal(certificates, environment.AllowedChainCertificates);
                Assert.Equal(tokenEndpoint, environment.TokenEndpoint);
            }

            [Fact]
            public void Gets_initialized_difi_test_environment()
            {
                //Arrange
                var url = new Uri("https://api.difitest.signering.posten.no");
                var certificates = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();
                var tokenEndpoint = new Uri("https://midp.difitest.digipost.no/oauth2/token");

                //Act
                var environment = Environment.DifiTest;

                //Assert
                Assert.Equal(url, environment.Url);
                Assert.Equal(certificates, environment.AllowedChainCertificates);
                Assert.Equal(tokenEndpoint, environment.TokenEndpoint);
            }

            [Fact]
            public void Gets_initialized_production_environment()
            {
                //Arrange
                var url = new Uri("https://api.signering.posten.no");
                var certificates = CertificateChainUtility.ProduksjonsSertifikater();
                var tokenEndpoint = new Uri("https://midp.digipost.no/oauth2/token");

                //Act
                var environment = Environment.Production;

                //Assert
                Assert.Equal(url, environment.Url);
                Assert.Equal(certificates, environment.AllowedChainCertificates);
                Assert.Equal(tokenEndpoint, environment.TokenEndpoint);
            }

            [Fact]
            public void Localhost_environment_has_no_token_endpoint()
            {
                //Arrange
                //Act
                var environment = Environment.Localhost;

                //Assert
                Assert.Null(environment.TokenEndpoint);
            }
        }
    }
}
