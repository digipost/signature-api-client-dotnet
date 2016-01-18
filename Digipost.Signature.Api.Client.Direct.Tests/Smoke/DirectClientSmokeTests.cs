using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Smoke
{
    [TestClass]
    public class DirectClientSmokeTests
    {
        [TestClass]
        public class CreateMethod : DirectClientSmokeTests
        {
            [TestMethod]
            public void SendsCreateSuccessfully()
            {
                //Arrange
                var sender = new Sender("983163327");
                var directClient = new DirectClient(
                    new ClientConfiguration(
                        new Uri("https://172.16.91.1:8443/"), 
                        sender, 
                        DomainUtility.GetTestIntegrasjonSertifikat()
                        )
                    );
                var directJob = new DirectJob(sender, DomainUtility.GetDocument(), DomainUtility.GetSigner(), "SmokeTestReference", DomainUtility.GetExitUrls());

                //Act
                directClient.Create(directJob);

                //Assert
            } 
        }
    }
}
