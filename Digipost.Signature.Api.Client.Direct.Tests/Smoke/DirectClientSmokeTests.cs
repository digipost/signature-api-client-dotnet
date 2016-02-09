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
        private string localStatusUrl = "https://172.16.91.1:8443/api/signature-jobs/141/status";
        private string difiTestStatusUrl = "https://api.difitest.signering.posten.no/api/signature-jobs/59/status";

        [TestClass]
        public class CreateMethod : DirectClientSmokeTests
        {
            [Ignore]
            [TestMethod]
            public async Task SendsCreateSuccessfully()
            {
                //Arrange
                var directClient = DirectClient();
                var directJob = new DirectJob(DomainUtility.GetDocument(), DomainUtility.GetSigner(), "SmokeTestReference", DomainUtility.GetExitUrls());

                //Act
                var result = await directClient.Create(directJob);

                //Assert
                Assert.IsNotNull(result.JobId);
            }

            [TestMethod]
            public async Task CreatesSuccessfullyForDifiTest()
            {
                //Arrange
                var client = DirectClientDifiTest();

                var result = await client.Create(DomainUtility.GetDirectJob());

                //Act

                //Assert
                Assert.IsNotNull(result.JobId);
            }
        }

        
        [TestClass]
        public class GetStatusMethod : DirectClientSmokeTests
        {
            [Ignore]
            [TestMethod]
            public async Task GetsStatusSuccessfully()
            {
                //Arrange
                var directClient = DirectClient();
                var directJob = new DirectJob(DomainUtility.GetDocument(), DomainUtility.GetSigner(), "SmokeTestReference", DomainUtility.GetExitUrls());
                var jobResponse = await directClient.Create(directJob);
                
                //Act
                var jobStatus = await directClient.GetStatus(jobResponse.StatusReference);

                //Assert
                Assert.IsNotNull(jobStatus.JobId);
            }

            [TestMethod]
            public async Task GetsStatusSuccessfullyForDifiTest()
            {
                //Arrange
                var directClient = DirectClientDifiTest();       

                //Act
                var jobStatus = await directClient.GetStatus(new StatusReference(new Uri(difiTestStatusUrl)));

                //Assert
                Assert.IsNotNull(jobStatus.JobId);
            }

        }

        [TestClass]
        public class GetXadesMethod : DirectClientSmokeTests
        {
            [Ignore]
            [TestMethod]
            public async Task GetsXadesSuccessfully()
            {
                //Arrange
                var directClient =  DirectClient();
                //var jobResponse = await directClient.Create(directJob);
                var directJobReference = new Direct.StatusReference(new Uri(localStatusUrl));
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

            [TestMethod]
            public async Task GetsXadesSuccessfullyForDifiTest()
            {
                //Arrange
                var directClient = DirectClientDifiTest();
                var jobStatus = await directClient.GetStatus(new StatusReference(new Uri(difiTestStatusUrl)));

                //Act
                var xades = await directClient.GetXades(jobStatus.JobReferences.Xades);

                //Assert
                Assert.IsNotNull(jobStatus.JobId);
            }

        }

        [TestClass]
        public class GetPadesMethod : DirectClientSmokeTests
        {
            [Ignore]
            [TestMethod]
            public async Task GetsPadesSuccessfully()
            {
                //Arrange
                var directClient = DirectClient();
                //var jobResponse = await directClient.Create(directJob);
                var directJobReference = new Direct.StatusReference(new Uri(localStatusUrl));
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

            [TestMethod]
            public async Task GetsPadesSuccessfullyForDifiTest()
            {
                //Arrange
                var directClient = DirectClientDifiTest();
                var jobStatus = await directClient.GetStatus(new StatusReference(new Uri(difiTestStatusUrl)));

                //Act
                var xades = await directClient.GetXades(jobStatus.JobReferences.Xades);

                //Assert
            }

        }

        [TestClass]
        public class ConfirmMethod : DirectClientSmokeTests
        {
            [Ignore]
            [TestMethod]
            public async Task ConfirmsSuccessfully()
            {
                //Arrange
                var directClient = DirectClient();
                var directJobReference = new Direct.StatusReference(new Uri(localStatusUrl));
                var jobstatus = await directClient.GetStatus(directJobReference);

                //Act
                await directClient.Confirm(jobstatus.JobReferences.Confirmation);

                //Assert
            }

            [TestMethod]
            public async Task ConfirmsSuccessfullyDifiTest()
            {
                //Arrange
                var directClient = DirectClientDifiTest();
                var jobStatus = await directClient.GetStatus(new StatusReference(new Uri(difiTestStatusUrl)));

                //Act
                var result = await directClient.Confirm(jobStatus.JobReferences.Confirmation);

                //Assert
                
            }

        }

        private static DirectClient DirectClientDifiTest()
        {
            var sender = new Sender("983163327");
            var clientConfig = new ClientConfiguration(new Uri("https://api.difitest.signering.posten.no"), sender,
                DomainUtility.GetTestIntegrasjonSertifikat());
            var client = new DirectClient(clientConfig);
            return client;
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
