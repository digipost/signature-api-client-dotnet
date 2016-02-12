using System;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Smoke
{
    [TestClass]
    public class DirectClientSmokeTests : SmokeTests
    {
        private const string LocalhostRelativeStatusUrl = "/api/988015814/direct/signature-jobs/78/status";
        private static readonly string DifitestSigneringPostenNoRelativeStatusUrl = "/api/signature-jobs/59/status";
        
        [TestClass]
        public class RunsEndpointCallsSuccessfully : DirectClientSmokeTests
        {
            [TestMethod]
            public async Task CreatesSuccessfully()
            {
                //Arrange
                var directClient = GetDirectClient();
                var directJob = DomainUtility.GetDirectJob();

                //Act
                var result = await directClient.Create(directJob);

                //Assert
                Assert.IsNotNull(result.JobId);
            }

            [TestMethod]
            public async Task GetsStatusSuccessfully()
            {
                //Arrange
                var directClient = GetDirectClient();

                //Act
                var directJobStatusResponse = await directClient.GetStatus(GetStatusReference());

                //Assert
                Assert.IsNotNull(directJobStatusResponse.JobId);
            }

            [TestMethod]
            public async Task GetsXadesSuccessfully()
            {
                //Arrange
                var directClient = GetDirectClient();
                var directJobStatusResponse = await directClient.GetStatus(GetStatusReference());
                var xadesReference = MorphDirectJobStatusResponseIfMayBe(directJobStatusResponse).References.Xades;

                //Act
                var xades = await directClient.GetXades(xadesReference);

                //Assert
                Assert.IsTrue(xades.CanRead);
            }

            [TestMethod]
            public async Task GetsPadesSuccessfully()
            {
                //Arrange
                var directClient = GetDirectClient();
                var directJobResponse = await directClient.GetStatus(GetStatusReference());
                var padesReference = MorphDirectJobStatusResponseIfMayBe(directJobResponse).References.Pades;

                //Act
                var pades = await directClient.GetPades(padesReference);

                //Assert
                Assert.IsTrue(pades.CanRead);
            }

            [TestMethod]
            public async Task ConfirmsSuccessfully()
            {
                //Arrange
                var directClient = GetDirectClient();
                var directJobResponse = await directClient.GetStatus(GetStatusReference());
                var confirmationReference = MorphDirectJobStatusResponseIfMayBe(directJobResponse).References.Confirmation;

                //Act
                var result = await directClient.Confirm(confirmationReference);

                //Assert
            }
        }

        private static StatusReference GetStatusReference()
        {
            Uri url;

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

        internal DirectJobStatusResponse MorphDirectJobStatusResponseIfMayBe(DirectJobStatusResponse directJobResponse)
        {
            switch (ClientType)
            {
                case Client.Localhost:
                    //Server returns Localhost, exchanges this with actual Localhost address
                    directJobResponse.References.Xades = new XadesReference(MorphLocalhostForVm(directJobResponse.References.Xades.Url));
                    directJobResponse.References.Pades = new PadesReference(MorphLocalhostForVm(directJobResponse.References.Pades.Url));
                    directJobResponse.References.Confirmation = new ConfirmationReference(MorphLocalhostForVm(directJobResponse.References.Confirmation.Url));
                    break;
                case Client.DifiTest:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return directJobResponse;
        }
    }
}
