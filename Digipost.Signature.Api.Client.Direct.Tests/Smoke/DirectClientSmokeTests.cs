using System;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.Direct.Tests.Smoke
{
    [TestClass]
    public class DirectClientSmokeTests : SmokeTests
    {
        private static StatusReference _statusReference;
        private static ConfirmationReference _confirmationReference;
        private static XadesReference _xadesReference;
        private static PadesReference _padesReference;

        protected static DirectClient GetDirectClient()
        {
            DirectClient client;

            switch (ClientType)
            {
                case Client.Localhost:
                    client = DirectClient(Environment.Localhost);
                    break;
                case Client.DifiTest:
                    client = DirectClient(Environment.DifiTest);
                    break;
                case Client.DifiQa:
                    client = DirectClient(Environment.DifiQa);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return client;
        }

        private static DirectClient DirectClient(Environment environment)
        {
            var sender = new Sender("988015814");
            var clientConfig = new ClientConfiguration(environment, sender, CoreDomainUtility.GetTestIntegrasjonSertifikat());
            var client = new DirectClient(clientConfig);
            return client;
        }

        internal static DirectJobStatusResponse MorphDirectJobStatusResponseIfMayBe(DirectJobStatusResponse directJobResponse)
        {
            switch (ClientType)
            {
                case Client.Localhost:
                    //Server returns 'localhost' as server address, while the server is running on vmWare hos address. We swap it here to avoid configuring server
                    directJobResponse.References.Xades = new XadesReference(GetUriFromRelativePath(directJobResponse.References.Xades.Url.AbsolutePath));
                    directJobResponse.References.Pades = new PadesReference(GetUriFromRelativePath(directJobResponse.References.Pades.Url.AbsolutePath));
                    directJobResponse.References.Confirmation = new ConfirmationReference(GetUriFromRelativePath(directJobResponse.References.Confirmation.Url.AbsolutePath));
                    break;
            }

            return directJobResponse;
        }

        [TestClass]
        public class RunsEndpointCallsSuccessfully : DirectClientSmokeTests
        {
            [ClassInitialize]
            public static void CreateAndGetStatus(TestContext context)
            {
                //Arrange
                var directClient = GetDirectClient();
                var directJob = DomainUtility.GetDirectJob();

                //Act
                var result = directClient.Create(directJob).Result;
                directClient.AutoSign(result.JobId).Wait();
                _statusReference = new StatusReference(GetUriFromRelativePath(result.ResponseUrls.Status.Url.AbsolutePath));

                var directJobStatusResponse = MorphDirectJobStatusResponseIfMayBe(directClient.GetStatus(_statusReference).Result);
                _xadesReference = directJobStatusResponse.References.Xades;
               _padesReference = directJobStatusResponse.References.Pades;
                _confirmationReference = directJobStatusResponse.References.Confirmation;

                //Assert
                Assert.IsNotNull(result.JobId);
                Assert.IsNotNull(_xadesReference);
                Assert.IsNotNull(_padesReference);
                Assert.IsNotNull(_confirmationReference);
            }

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
                var directJobStatusResponse = await directClient.GetStatus(_statusReference);

                //Assert
                Assert.IsNotNull(directJobStatusResponse.JobId);
            }

            [TestMethod]
            public async Task GetsXadesSuccessfully()
            {
                //Arrange
                var directClient = GetDirectClient();

                //Act
                var xades = await directClient.GetXades(_xadesReference);

                //Assert
                Assert.IsTrue(xades.CanRead);
            }

            [TestMethod]
            public async Task GetsPadesSuccessfully()
            {
                //Arrange
                var directClient = GetDirectClient();

                //Act
                var pades = await directClient.GetPades(_padesReference);

                //Assert
                Assert.IsTrue(pades.CanRead);
            }

            [TestMethod]
            public async Task ConfirmsSuccessfully()
            {
                //Arrange
                var directClient = GetDirectClient();

                //Act
                await directClient.Confirm(_confirmationReference);

                //Assert
            }
        }
    }
}