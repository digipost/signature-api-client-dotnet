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
            public async Task SendsCreateSuccessfullyForDifiTest()
            {
                //Arrange
                var portalClient = DirectClientDifiTest();
                var portalJob = DomainUtility.GetPortalJob();
                var result = await portalClient.Create(portalJob);

                //Act

                //Assert
            } 
        }

        private static PortalClient DirectClientDifiTest()
        {
            var sender = new Sender("983163327");
            var clientConfig = new ClientConfiguration(new Uri("https://api.difitest.signering.posten.no"), sender, DomainUtility.GetTestIntegrasjonSertifikat());
            var client = new PortalClient(clientConfig);
            return client;
        }
    }
}
