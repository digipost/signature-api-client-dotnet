using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Exceptions;
using Digipost.Signature.Api.Client.Portal.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    [TestClass]
    public class PortalClientTests
    {
        internal HttpClient GetHttpClientWithHandler(DelegatingHandler delegatingHandler)
        {
            return new HttpClient(delegatingHandler)
            {
                BaseAddress = new Uri("http://mockUrl.no")
            };
        }

        [TestClass]
        public class GetStatusChangeMethod : PortalClientTests
        {
            [TestMethod]
            public async Task ReturnsNullOnEmptyQueue()
            {
                //Arrange
                var portalClient = new PortalClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForEmptyQueueResponse())
                };
                object expectedResponse = null;

                //Act
                var actualResponse = await portalClient.GetStatusChange();

                //Assert
                Assert.AreEqual(expectedResponse, actualResponse);
            }

            [TestMethod]
            public async Task ReturnsPortalJobStatusChangeOnOkResponse()
            {
                //Arrange
                var portalClient = new PortalClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForJobStatusChangeResponse())
                };

                object expectedResponseType = typeof (PortalJobStatusChangeResponse);

                //Act
                var actualResponseType = (await portalClient.GetStatusChange()).GetType();

                //Assert
                Assert.AreEqual(expectedResponseType, actualResponseType);
            }

            [TestMethod]
            [ExpectedException(typeof (TooEagerPollingException))]
            public async Task ThrowsExceptionOnTooManyRequests()
            {
                //Arrange
                var portalClient = new PortalClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForTooManyRequestsResponse())
                };

                //Act
                await portalClient.GetStatusChange();

                //Assert
            }

            [TestMethod]
            [ExpectedException(typeof (UnexpectedResponseException))]
            public async Task ThrowsUnexpectedExceptionWithErrorClassOnUnexpectedError()
            {
                //Arrange
                var portalClient = new PortalClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHanderForErrorResponse())
                };

                //Act
                await portalClient.GetStatusChange();

                //Assert
            }

            [TestMethod]
            [ExpectedException(typeof (BrokerNotAuthorizedException))]
            public async Task ThrowsBrokerNotAuthorizedExceptionOnNotAuthorized()
            {
                //Arrange
                var portalClient = new PortalClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHanderForBrokerNotAuthorizedErrorResponse())
                };

                //Act
                await portalClient.GetStatusChange();

                //Assert
            }
        }

        [TestClass]
        public class CancelMethod : PortalClientTests
        {
            [ExpectedException(typeof (JobCompletedException))]
            [TestMethod]
            public async Task ThrowsJobCompletedExceptionOnConflict()
            {
                //Arrange
                var portalClient = new PortalClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForJobCompletedResponse())
                };

                //Act
                await portalClient.Cancel(new CancellationReference(new Uri("http://cancellationuri.no")));

                //Assert
            }

            [ExpectedException(typeof (UnexpectedResponseException))]
            [TestMethod]
            public async Task ThrowsUnexpectedErrorOnUnexpectedErrorCode()
            {
                //Arrange
                var portalClient = new PortalClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForInternalServerErrorResponse())
                };

                //Act
                await portalClient.Cancel(new CancellationReference(new Uri("http://cancellationuri.no")));

                //Assert
            }
        }
    }
}