using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Tests.Fakes;
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
                var clientConfiguration = DomainUtility.GetClientConfiguration();
                var directClient = new DirectClient(clientConfiguration);
                var signatureServiceRootUri = DomainUtility.GetSignatureServiceRootUri();

                directClient.HttpClient = new HttpClient(new FakeHttpClientHandlerForDirectCreateResponse())
                {
                    BaseAddress = signatureServiceRootUri
                };

                var directJob = DomainUtility.GetDirectJob();

                //Act
                var result = await directClient.Create(directJob);

                //Assert
                Assert.IsNotNull(result.Content.ReadAsStringAsync());
                Assert.Fail(); //TODO: Check correct return type.
            }
        }
    }
}
