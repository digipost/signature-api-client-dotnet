using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Tests.Fakes;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Integration
{
    [TestClass]
    public class DirectClientIntegrationTests
    {
        [TestClass]
        public class CreateMethod : DirectClientTests
        {
            [TestMethod]
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
                var result = await directClient.Create(directJob);

                //Assert
                Assert.IsNotNull(result.JobId);
            }
        }

        [TestClass]
        public class GetStatusMethod : DirectClientTests
        {
            [TestMethod]
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
                var result = await directClient.GetStatus(directJobResponse.ResponseUrls.Status);

                //Assert
                Assert.IsNotNull(result.JobId);
            }
        }

    }
}
