using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests.Smoke
{
    [TestClass]
    public class PortalClientSmokeTests : SmokeTests
    {

        private const string LocalhostRelativeStatusUrl = "/api/988015814/direct/signature-jobs/56/status";
        private static readonly string DifitestSigneringPostenNoRelativeStatusUrl = "/api/signature-jobs/59/status";


        [TestClass]
        public class RunsEndpointCallsSuccessfully : PortalClientSmokeTests
        {
            [TestMethod]
            public async Task CreatesSuccessfully()
            {
                //Arrange
                var portalClient = GetPortalClient();
                var portalJob = DomainUtility.GetPortalJob();

                //Act
                var result = await portalClient.Create(portalJob);

                //Assert
                Assert.IsNotNull(result.JobId);
            }

            [TestMethod]
            public async Task GetsStatusSuccessfully()
            {
                //Arrange

                //Act

                //Assert
            }

            [TestMethod]
            public async Task GetsXadesSuccessfully()
            {
                //Arrange

                //Act


                //Assert
            }


            [TestMethod]
            public async Task GetsPadesSuccessfully()
            {
                //Arrange
                
                //Act

                //Assert
            }

            [TestMethod]
            public async Task ConfirmsSuccessfully()
            {
                //Arrange

                //Act

                //Assert
            }
        }
    }
}
