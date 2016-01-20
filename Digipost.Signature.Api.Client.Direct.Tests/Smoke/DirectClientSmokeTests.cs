using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
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
            public async Task SendsCreateSuccessfully()
            {
                //Arrange
                var sender = new Sender("983163327");
                var directClient = new DirectClient(
                    new ClientConfiguration(
                        new Uri("https://172.16.91.1:8443"),
                        sender,
                        DomainUtility.GetTestIntegrasjonSertifikat()
                        )
                    );
                var directJob = new DirectJob(sender, DomainUtility.GetDocument(), DomainUtility.GetSigner(), "SmokeTestReference", DomainUtility.GetExitUrls());

                //Act
                var result = await directClient.Create(directJob);
                Trace.WriteLine("result is:" + result.Content.ReadAsStringAsync().Result + "Reason:" + result.ReasonPhrase);

                //Assert
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            }
        }
    }
}
