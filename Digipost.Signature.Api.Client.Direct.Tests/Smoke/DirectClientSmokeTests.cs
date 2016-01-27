using System;
using System.IO;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Smoke
{
    [TestClass]
    public class DirectClientSmokeTests
    {
        [Ignore]
        [TestClass]
        public class CreateMethod : DirectClientSmokeTests
        {
            [TestMethod]
            public async Task SendsCreateSuccessfully()
            {
                //Arrange
                var directClient = DirectClient();
                var directJob = new DirectJob(DomainUtility.GetSender(), DomainUtility.GetDocument(), DomainUtility.GetSigner(), "SmokeTestReference", DomainUtility.GetExitUrls());

                //Act
                var result = await directClient.Create(directJob);

                //Assert
                Assert.IsNotNull(result.JobId);
            }
        }

        [Ignore]
        [TestClass]
        public class GetStatusMethod : DirectClientSmokeTests
        {
            [TestMethod]
            public async Task GetsStatusSuccessfully()
            {
                //Arrange
                var directClient = DirectClient();
                var directJob = new DirectJob(DomainUtility.GetSender(), DomainUtility.GetDocument(), DomainUtility.GetSigner(), "SmokeTestReference", DomainUtility.GetExitUrls());
                var jobResponse = await directClient.Create(directJob);
                
                //Act
                var jobStatus = await directClient.GetStatus(jobResponse.DirectJobReference);

                //Assert
                Assert.IsNotNull(jobStatus.JobId);
            }
        }

        [Ignore]
        [TestClass]
        public class GetXadesMethod : DirectClientSmokeTests
        {
            [TestMethod]
            public async Task GetsXadesSuccessfully()
            {
                //Arrange
                var directJob = DomainUtility.GetDirectJob();
                var directClient = DirectClient();
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

        [Ignore]
        [TestClass]
        public class GetPadesMethod : DirectClientSmokeTests
        {
            [TestMethod]
            public async Task GetsPadesSuccessfully()
            {
                //Arrange
                var directClient = DirectClient();
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

        [TestClass]
        public class ConfirmMethod : DirectClientSmokeTests
        {
            [TestMethod]
            public async Task ConfirmsSuccessfully()
            {
                //Arrange
                var directClient = DirectClient();
                var directJobReference = new DirectJobReference(new Uri("https://172.16.91.1:8443/api/signature-jobs/121/status"));
                var jobstatus = await directClient.GetStatus(directJobReference);

                //Act
                await directClient.Confirm(jobstatus);

                //Assert
            } 
        }


        private static DirectClient DirectClient()
        {
            var sender = DomainUtility.GetSender();

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
