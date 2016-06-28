using System;
using System.Threading.Tasks;
using System.Web;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.Direct.Tests.Smoke
{
    [TestClass]
    public class DirectClientSmokeTests : SmokeTests
    {
        private static DirectClient _directClient;

        private static StatusReference _statusReference;
        private static ConfirmationReference _confirmationReference;
        private static XadesReference _xadesReference;
        private static PadesReference _padesReference;

        protected static DirectClient GetDirectClient()
        {
            if (_directClient != null)
            {
                return _directClient;
            }

            switch (ClientType)
            {
                case Client.Localhost:
                    _directClient = DirectClient(Environment.Localhost);
                    break;
                case Client.DifiTest:
                    _directClient = DirectClient(Environment.DifiTest);
                    break;
                case Client.DifiQa:
                    _directClient = DirectClient(Environment.DifiQa);
                    break;
                case Client.Test:
                    var testEnvironment = Environment.DifiTest;
                    testEnvironment.Url = new Uri(Environment.DifiQa.Url.AbsoluteUri.Replace("difiqa", "test"));
                    _directClient = DirectClient(testEnvironment);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _directClient;
        }

        private static DirectClient DirectClient(Environment environment)
        {
            var sender = new Sender("988015814");
            var clientConfig = new ClientConfiguration(environment, CoreDomainUtility.GetTestIntegrasjonSertifikat(), sender);
            var client = new DirectClient(clientConfig);
            return client;
        }

        internal static JobStatusResponse MorphJobStatusResponseIfMayBe(JobStatusResponse jobStatusResponse)
        {
            switch (ClientType)
            {
                case Client.Localhost:
                    //Server returns 'localhost' as server address, while the server is running on vmWare hos address. We swap it here to avoid configuring server
                    jobStatusResponse.References.Xades = new XadesReference(GetUriFromRelativePath(jobStatusResponse.References.Xades.Url.AbsolutePath));
                    jobStatusResponse.References.Pades = new PadesReference(GetUriFromRelativePath(jobStatusResponse.References.Pades.Url.AbsolutePath));
                    jobStatusResponse.References.Confirmation = new ConfirmationReference(GetUriFromRelativePath(jobStatusResponse.References.Confirmation.Url.AbsolutePath));
                    break;
            }

            return jobStatusResponse;
        }

        internal static StatusReference MorphStatusReferenceIfMayBe(StatusReference statusReference)
        {
            var statusReferenceUri = GetUriFromRelativePath(statusReference.Url().AbsolutePath);
            return new StatusReference(statusReferenceUri, statusReference.StatusQueryToken);
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
                var directJobResponse = directClient.Create(directJob).Result;
                var statusQueryToken = AutoSignAndGetToken(directClient, directJobResponse).Result;
                _statusReference = MorphStatusReferenceIfMayBe(directJobResponse.ResponseUrls.Status(statusQueryToken));

                var jobStatusResponse = directClient.GetStatus(_statusReference).Result;

                var morphedJobStatusResponse = MorphJobStatusResponseIfMayBe(jobStatusResponse);
                _xadesReference = morphedJobStatusResponse.References.Xades;
                _padesReference = morphedJobStatusResponse.References.Pades;
                _confirmationReference = morphedJobStatusResponse.References.Confirmation;

                //Assert
                Assert.IsNotNull(_statusReference);
                Assert.IsNotNull(directJobResponse.JobId);
                Assert.IsNotNull(_xadesReference);
                Assert.IsNotNull(_padesReference);
                Assert.IsNotNull(_confirmationReference);
            }

            private static async Task<string> AutoSignAndGetToken(DirectClient directClient, JobResponse jobResponse)
            {
                var statusUrl = await directClient.AutoSign(jobResponse.JobId);
                var queryParams = new Uri(statusUrl).Query;
                var queryDictionary = HttpUtility.ParseQueryString(queryParams);
                var statusQueryToken = queryDictionary.Get(0);
                return statusQueryToken;
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
                var jobStatusResponse = await directClient.GetStatus(_statusReference);

                //Assert
                Assert.IsNotNull(jobStatusResponse.JobId);
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
            public async Task ConfirmsSuccessfully()
            {
                //Arrange
                var directClient = GetDirectClient();

                //Act
                await directClient.Confirm(_confirmationReference);

                //Assert
            }
        }

        [TestClass]
        public class StatusCanBeRetrievedByPolling : DirectClientSmokeTests
        {

            private static JobResponse _createdPollableJob;

            [ClassInitialize]
            public static void CreatePollableJob(TestContext context)
            {
                var directClient = GetDirectClient();
                var directJob = DomainUtility.GetPollableDirectJob();

                _createdPollableJob = directClient.Create(directJob).Result;

                directClient.AutoSign(_createdPollableJob.JobId).Wait();
            }

            [TestMethod]
            public void ReturnsSignedJobWhenPolling()
            {
                var directClient = GetDirectClient();

                var statusChange = directClient.GetStatusChange().Result;

                Assert.AreEqual(JobStatus.Signed, statusChange.Status);
                Assert.AreEqual(_createdPollableJob.JobId, statusChange.JobId);

                var morphedJobStatusResponse = MorphJobStatusResponseIfMayBe(statusChange);
                directClient.Confirm(morphedJobStatusResponse.References.Confirmation).Wait();
            }
        }

    }
}