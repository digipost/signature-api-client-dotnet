using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Tests.Fakes;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests.Integration
{
    public class DirectClientIntegrationTests
    {
        public class CreateMethod : DirectClientTests
        {
            [Fact]
            public async Task SendsSuccessfully()
            {
                //Arrange
                var clientConfiguration = CoreDomainUtility.GetClientConfiguration();
                var directClient = new DirectClient(clientConfiguration)
                {
                    HttpClient = new HttpClient(new FakeHttpClientHandlerForDirectCreateResponse())
                    {
                        BaseAddress = new Uri("http://fakesignatureserviceroot.digipost.no")
                    }
                };
                var directJob = DomainUtility.GetDirectJob();

                //Act
                var result = await directClient.Create(directJob).ConfigureAwait(false);

                //Assert
                Assert.NotNull(result);
            }
        }

        public class GetStatusMethod : DirectClientTests
        {
            [Fact]
            public async Task SendsSuccessfully()
            {
                //Arrange
                var clientConfiguration = CoreDomainUtility.GetClientConfiguration();
                var directClient = new DirectClient(clientConfiguration)
                {
                    HttpClient = new HttpClient(new FakeHttpClientHandlerGetStatusResponse())
                    {
                        BaseAddress = new Uri("http://fakesignatureserviceroot.digipost.no")
                    }
                };

                var directJobResponse = DomainUtility.GetDirectJobResponse();

                //Act
                var result = await directClient.GetStatus(directJobResponse.ResponseUrls.Status("StatusQueryToken")).ConfigureAwait(false);

                //Assert
                Assert.NotNull(result);
            }
        }
    }
}