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
        private string statusUrl = "https://172.16.91.1:8443/api/signature-jobs/141/status";

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
                var jobStatus = await directClient.GetStatus(jobResponse.StatusReference);

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
                var directClient =  DirectClient();
                //var jobResponse = await directClient.Create(directJob);
                var directJobReference = new DirectJobReference(new Uri(statusUrl));
                var jobStatus = await directClient.GetStatus(directJobReference);

                //Act
                using (var xadesStream = await directClient.GetXades(jobStatus.JobReferences.Xades))
                {
                    using (var memoryStream = new MemoryStream()){
                        xadesStream.CopyTo(memoryStream);
                        File.WriteAllBytes(@"C:\Users\aas\Downloads\xades.xml", memoryStream.ToArray());
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
                var directJobReference = new DirectJobReference(new Uri(statusUrl));
                var jobStatus = await directClient.GetStatus(directJobReference);

                //Act
                using (var padesStream = await directClient.GetPades(jobStatus.JobReferences.Pades))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        padesStream.CopyTo(memoryStream);
                        File.WriteAllBytes(@"C:\Users\aas\Downloads\padex.pdf", memoryStream.ToArray());
                    }
                };

                //Assert
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
                var directJobReference = new DirectJobReference(new Uri(statusUrl));
                var jobstatus = await directClient.GetStatus(directJobReference);

                //Act
                await directClient.Confirm(jobstatus.JobReferences.Confirmation);

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
