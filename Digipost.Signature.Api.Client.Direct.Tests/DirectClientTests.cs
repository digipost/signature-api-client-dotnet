using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Direct.Tests.Fakes;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Digipost.Signature.Api.Client.Core.Tests.Utilities.CoreDomainUtility;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class DirectClientTests
    {
        [TestClass]
        public class ConstructorMethod : DirectClientTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var clientConfiguration = GetClientConfiguration();

                //Act
                var client = new DirectClient(clientConfiguration);

                //Assert
                Assert.AreEqual(clientConfiguration, client.ClientConfiguration);
                Assert.IsNotNull(client.HttpClient);
                Assert.IsNotNull(client.HttpClient);
            }
        }

        [TestClass]
        public class CreateMethod : DirectClientTests
        {
            [TestMethod]
            [ExpectedException(typeof(SenderNotSpecifiedException))]
            public async Task ThrowsExceptionOnNoSender()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, GetPostenTestCertificate());
                var directClient = new DirectClient(clientConfiguration);
                var directJob = new Job(DomainUtility.GetDirectDocument(), DomainUtility.GetSigner(), "SendersReference", DomainUtility.GetExitUrls());

                //Act
                await directClient.Create(directJob);

                //Assert
                Assert.Fail();
            }
        }

        [TestClass]
        public class GetStatusMethod : DirectClientTests
        {
            [TestMethod]
            public async Task ReturnsStatusResponse()
            {
                //Arrange
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = new HttpClient(new FakeHttpClientHandlerGetStatusResponse())
                };

                //Act
                var result = await directClient.GetStatus(new StatusReference(new Uri("http://statusReference.no"), "StatusQueryToken"));

                //Assert
                Assert.IsNotNull(result);
            }

            [TestMethod]
            [ExpectedException(typeof(SignatureException), AllowDerivedTypes = true)]
            public async Task GetStatusThrowsSignatureException()
            {
                //Arrange
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = new HttpClient(new FakeHttpClientHandlerForInternalServerErrorResponse())
                };

                //Act
                await directClient.GetStatus(new StatusReference(new Uri("http://statusReference.no"), "StatusQueryToken"));

                //Assert
                Assert.Fail();
            }
        }

        [TestClass]
        public class GetStatusChangeMethod : DirectClientTests
        {
            [TestMethod]
            public async Task ReturnsEmptyObjectOnEmptyQueue()
            {
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForEmptyQueueResponse())
                };

                var actualResponse = await directClient.GetStatusChange();

                Assert.AreEqual(JobStatusResponse.NoChanges, actualResponse);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public async Task CantGetSignatureJobIdFromEmptyResponse()
            {
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForEmptyQueueResponse())
                };

                var statusChange = await directClient.GetStatusChange();
                var invalidOperation = statusChange.JobId;
            }

            [TestMethod]
            [ExpectedException(typeof(TooEagerPollingException))]
            public async Task ThrowsExceptionOnTooManyRequests()
            {
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForTooManyRequestsResponse())
                };

                await directClient.GetStatusChange();

                Assert.Fail("Should fail with " + typeof(TooEagerPollingException).Name);
            }

            [TestMethod]
            public async Task ReturnsStatusResponse()
            {
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerGetStatusResponse())
                };

                var result = await directClient.GetStatusChange();

                Assert.IsNotNull(result);
            }
        }
    }
}