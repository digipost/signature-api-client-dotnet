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
            public async Task SendsCreateSuccessfullyToDifiTest()
            {
                //Arrange
                var portalClient = DirectClientDifiTest();
                var portalJob = DomainUtility.GetPortalJob();
                var result = await portalClient.Create(portalJob);



                //Act

                //Assert
                Assert.IsNotNull(result.JobId);
            } 
        }

        private static PortalClient DirectClientDifiTest()
        {
            var sender = new Sender("983163327");
            var clientConfig = new ClientConfiguration(new Uri("https://api.difitest.signering.posten.no"), sender, DomainUtility.GetTestIntegrasjonSertifikat());
            var client = new PortalClient(clientConfig);
            return client;
        }

        private static PortalClient DirectClientLocalhost()
        {
            var sender = new Sender("983163327");
            var clientConfig = new ClientConfiguration(new Uri("https://172.16.91.1:8443"), sender, DomainUtility.GetTestIntegrasjonSertifikat());
            var client = new PortalClient(clientConfig);
            return client;
        }

    }
}
