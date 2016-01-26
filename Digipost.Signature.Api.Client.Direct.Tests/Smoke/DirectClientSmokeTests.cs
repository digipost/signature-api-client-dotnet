using System;
using System.IO;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Smoke
{
    [Ignore]
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
                var directClient = DirectClient(sender);
                var directJob = new DirectJob(sender, DomainUtility.GetDocument(), DomainUtility.GetSigner(), "SmokeTestReference", DomainUtility.GetExitUrls());

                //Act
                var result = await directClient.Create(directJob);

                //Assert
                Assert.IsNotNull(result.JobId);
            }
        }

        [TestClass]
        public class GetStatusMethod : DirectClientSmokeTests
        {
            [TestMethod]
            public async Task GetsStatusSuccessfully()
            {
                //Arrange
                var sender = new Sender("983163327");
                var directClient = DirectClient(sender);
                var directJob = new DirectJob(sender, DomainUtility.GetDocument(), DomainUtility.GetSigner(), "SmokeTestReference", DomainUtility.GetExitUrls());
                var jobResponse = await directClient.Create(directJob);
                
                //Act
                var jobStatus = await directClient.GetStatus(jobResponse.DirectJobReference);

                //Assert
                Assert.IsNotNull(jobStatus.JobId);
            }
        }
        
        [TestClass]
        public class GetXadesMethod : DirectClientSmokeTests
        {
            [TestMethod]
            public async Task GetsXadesSuccessfully()
            {
                //Arrange
                var sender = DomainUtility.GetSender();
                var directJob = DomainUtility.GetDirectJob();
                var directClient = DirectClient(sender);
                //var jobResponse = await directClient.Create(directJob);
                var directJobReference = new DirectJobReference(new Uri("https://172.16.91.1:8443/api/signature-jobs/121/status"));
                var jobStatus = await directClient.GetStatus(directJobReference);

                //Act
                var xadesReference = new XadesReference(jobStatus.StatusResponseUrls.Xades);
                using (Stream xadesStream = await directClient.GetXades(xadesReference))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        xadesStream.CopyTo(ms);
                        File.WriteAllBytes(@"C:\Users\aas\Downloads\xades.xml", ms.ToArray());
                    }
                };

                //Assert
            }
        }

        [TestClass]
        public class GetPadesMethod : DirectClientSmokeTests
        {
            [TestMethod]
            public async Task GetsPadesSuccessfully()
            {
                //Arrange
                var sender = DomainUtility.GetSender();
                var directJob = DomainUtility.GetDirectJob();
                var directClient = DirectClient(sender);
                //var jobResponse = await directClient.Create(directJob);
                var directJobReference = new DirectJobReference(new Uri("https://172.16.91.1:8443/api/signature-jobs/121/status"));
                var jobStatus = await directClient.GetStatus(directJobReference);

                //Act
                var padesReference = new PadesReference(jobStatus.StatusResponseUrls.Pades);
                using (Stream xadesStream = await directClient.GetPades(padesReference))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        xadesStream.CopyTo(ms);
                        File.WriteAllBytes(@"C:\Users\aas\Downloads\pades2.pdf", ms.ToArray());
                    }
                };
            }
        }


        private static DirectClient DirectClient(Sender sender)
        {
            var directClient = new DirectClient(
                new ClientConfiguration(
                    new Uri("https://172.16.91.1:8443"),
                    sender,
                    DomainUtility.GetTestIntegrasjonSertifikat()
                    )
                );
            return directClient;
        }
    }
}
