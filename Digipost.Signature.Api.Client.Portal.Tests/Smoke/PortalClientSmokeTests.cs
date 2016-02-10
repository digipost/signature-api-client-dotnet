using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests.Smoke
{
    [TestClass]
    public class PortalClientSmokeTests
    {
        [TestClass]
        public class CreateMethod : PortalClientSmokeTests
        {
            [TestMethod]
            public async Task SendsCreateSuccessfully()
            {
                //Arrange
                var portalClient = PortalClientDifiTest();
                var portalJob = DomainUtility.GetPortalJob();
                var result = await portalClient.Create(portalJob);
                
                //Act

                //Assert
                Assert.IsNotNull(result.JobId);
            }

            [TestMethod]
            public async Task SendsGetStatusChangeSuccessfully()
            {
                //Arrange
                var portalClient = PortalClientDifiTest();
                var result = await portalClient.GetStatusChange();

                //Act

                //Assert
                //Assert.IsNotNull(result.JobId);
            }

        }

        private static PortalClient PortalClientDifiTest()
        {
            var sender = new Sender("983163327");
            var clientConfig = new ClientConfiguration(new Uri("https://api.difitest.signering.posten.no"), sender, DomainUtility.GetTestIntegrasjonSertifikat());
            var client = new PortalClient(clientConfig);
            return client;
        }

        private static PortalClient PortalClientLocalhost()
        {
            var sender = new Sender("983163327");
            var clientConfig = new ClientConfiguration(new Uri("https://172.16.91.1:8443"), sender, DomainUtility.GetTestIntegrasjonSertifikat());
            var client = new PortalClient(clientConfig);
            return client;
        }

    }
}
