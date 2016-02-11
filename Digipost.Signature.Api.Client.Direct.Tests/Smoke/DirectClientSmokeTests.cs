using System;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Smoke
{
    [TestClass]
    public class DirectClientSmokeTests
    {
        private static readonly Client ClientType = Client.Localhost;

        private static readonly Uri Localhost = new Uri("https://172.16.91.1:8443");
        private const string LocalhostRelativeStatusUrl = "/api/988015814/direct/signature-jobs/56/status";

        private static readonly Uri DifitestSigneringPostenNo = new Uri("https://api.difitest.signering.posten.no");
        private static readonly string DifitestSigneringPostenNoRelativeStatusUrl = "/api/signature-jobs/59/status";


        private enum Client
        {
            Localhost,
            DifiTest
        }
        
        [TestClass]
        public class RunsEndpointCallsSuccessfully : DirectClientSmokeTests
        {
            [TestMethod]
            public async Task CreatesSuccessfully()
            {
                //Arrange

                //Act
                var result = await GetClient().Create(DomainUtility.GetDirectJob());

                //Assert
                Assert.IsNotNull(result.JobId);
            }

            [TestMethod]
            public async Task GetsStatusSuccessfully()
            {
                //Arrange
                var directClient = DirectClientDifiTest();

                //Act
                var jobStatus = await directClient.GetStatus(GetStatusReference());

                //Assert
                Assert.IsNotNull(jobStatus.JobId);
            }

            [TestMethod]
            public async Task GetsXadesSuccessfully()
            {
                //Arrange
                var directClient = DirectClientDifiTest();
                var jobStatus = await directClient.GetStatus(GetStatusReference());

                //Act
                var xades = await directClient.GetXades(GetXadesReference(jobStatus));

                //Assert
                Assert.IsTrue(xades.CanRead);
            }


            [TestMethod]
            public async Task GetsPadesSuccessfully()
            {
                //Arrange
                var directClient = DirectClientDifiTest();
                var jobStatus = await directClient.GetStatus(GetStatusReference());

                //Act
                var pades = await directClient.GetPades(GetPadesReference(jobStatus));

                //Assert
            }

            [TestMethod]
            public async Task ConfirmsSuccessfully()
            {
                //Arrange
                var directClient = DirectClientDifiTest();
                var jobStatus = await directClient.GetStatus(GetStatusReference());

                //Act
                var result = await directClient.Confirm(GetConfirmationReference(jobStatus));

                //Assert
            }
        }

        public static DirectClient GetClient()
        {
            DirectClient directClient = null;

            switch (ClientType)
            {
                case Client.Localhost:
                    directClient = LocalhostClient();
                    break;
                case Client.DifiTest:
                    directClient = DirectClientDifiTest();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return directClient;
        }

        private static StatusReference GetStatusReference()
        {
            Uri url = null;

            switch (ClientType)
            {
                case Client.Localhost:
                    url = new Uri(Localhost, LocalhostRelativeStatusUrl);
                    break;
                case Client.DifiTest:
                    url = new Uri(DifitestSigneringPostenNo, LocalhostRelativeStatusUrl);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new StatusReference(url);
        }


        private static DirectClient DirectClientDifiTest()
        {
            var sender = new Sender("983163327");
            var clientConfig = new ClientConfiguration(DifitestSigneringPostenNo, sender, DomainUtility.GetTestIntegrasjonSertifikat());
            var client = new DirectClient(clientConfig);
            return client;
        }


        private static DirectClient LocalhostClient()
        {
            var sender = DomainUtility.GetSender();

            var directClient = new DirectClient(new ClientConfiguration(Localhost, sender, DomainUtility.GetTestIntegrasjonSertifikat()));
            return directClient;
        }

        private static XadesReference GetXadesReference(DirectJobStatusResponse jobStatus)
        {
            XadesReference xadesReference = null;

            switch (ClientType)
            {
                case Client.Localhost:
                    xadesReference = new XadesReference(new Uri(Localhost, jobStatus.References.Xades.Url.AbsolutePath)); //Server returns Localhost, exchanges this with actual Localhost address
                    break;
                case Client.DifiTest:
                    xadesReference = jobStatus.References.Xades;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return xadesReference;
        }

        private static PadesReference GetPadesReference(DirectJobStatusResponse directJobResponse)
        {
            PadesReference padesReference = null;

            switch (ClientType)
            {
                case Client.Localhost:
                    padesReference = new PadesReference(new Uri(Localhost, directJobResponse.References.Pades.Url.AbsolutePath)); //Server returns Localhost, exchanges this with actual Localhost address
                    break;
                case Client.DifiTest:
                    padesReference = directJobResponse.References.Pades;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return padesReference;
        }

        private ConfirmationReference GetConfirmationReference(DirectJobStatusResponse directJobResponse)
        {
            ConfirmationReference confirmationReference = null;
            switch (ClientType)
            {
                case Client.Localhost:
                    confirmationReference = new ConfirmationReference(new Uri(Localhost, directJobResponse.References.Confirmation.Url.AbsolutePath)); //Server returns Localhost, exchanges this with actual Localhost address;
                    break;
                case Client.DifiTest:
                    confirmationReference = directJobResponse.References.Confirmation;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return confirmationReference;
        }
    }
}
