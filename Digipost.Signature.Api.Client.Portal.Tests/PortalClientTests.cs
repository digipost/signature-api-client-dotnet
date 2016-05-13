using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Exceptions;
using Digipost.Signature.Api.Client.Portal.Tests.Fakes;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

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
        public class CreateMethod : PortalClientTests
        {
            [TestMethod]
            [ExpectedException(typeof (SenderNotSpecifiedException))]
            public async Task ThrowsExceptionOnNoSender()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate());
                var portalClient = new PortalClient(clientConfiguration);
                var portalJob = new PortalJob(DomainUtility.GetPortalDocument(), DomainUtility.GetSigners(1), "SendersReference");

                //Act
                await portalClient.Create(portalJob);

                //Assert
                Assert.Fail();
            }
        }

        [TestClass]
        public class GetStatusChangeMethod : PortalClientTests
        {
            [TestMethod]
            [ExpectedException(typeof (SenderNotSpecifiedException))]
            public async Task ThrowsExceptionOnSenderNotSpecified()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate());
                var fakeHttpClientHandlerChecksCorrectSender = new FakeHttpClientHandlerForJobStatusChangeResponse();
                var portalClient = new PortalClient(clientConfiguration)
                {
                    HttpClient = GetHttpClientWithHandler(fakeHttpClientHandlerChecksCorrectSender)
                };

                //Act
                await portalClient.GetStatusChange();

                //Assert
                Assert.Fail();
            }

            [TestMethod]
            public async Task CanBeCalledWithoutSenderUsesSenderInClientConfiguration()
            {
                //Arrange
                var sender = CoreDomainUtility.GetSender();
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate(), sender);
                var fakeHttpClientHandlerChecksCorrectSender = new FakeHttpClientHandlerChecksCorrectSenderResponse();
                var portalClient = new PortalClient(clientConfiguration)
                {
                    HttpClient = GetHttpClientWithHandler(fakeHttpClientHandlerChecksCorrectSender)
                };

                //Act
                await portalClient.GetStatusChange();

                //Assert
                Assert.IsTrue(fakeHttpClientHandlerChecksCorrectSender.RequestUri.Contains(sender.OrganizationNumber));
            }

            [TestMethod]
            public async Task CalledWithBothSendersUsesInput()
            {
                //Arrange
                var parameterSender = new Sender("000000000");
                var clientConfigurationSender = new Sender("999999999");
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate(), clientConfigurationSender);
                var fakeHttpClientHandlerChecksCorrectSender = new FakeHttpClientHandlerChecksCorrectSenderResponse();
                var portalClient = new PortalClient(clientConfiguration)
                {
                    HttpClient = GetHttpClientWithHandler(fakeHttpClientHandlerChecksCorrectSender)
                };

                //Act
                await portalClient.GetStatusChange(parameterSender);

                //Assert
                Assert.IsTrue(fakeHttpClientHandlerChecksCorrectSender.RequestUri.Contains(parameterSender.OrganizationNumber));
            }

            [TestMethod]
            public async Task ReturnsEmptyObjectOnEmptyQueue()
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
                Assert.AreEqual(JobStatusChanged.NoChangesJobStatusChanged, actualResponse);
            }

            [TestMethod]
            public async Task ReturnsPortalJobStatusChangeOnOkResponse()
            {
                //Arrange
                var portalClient = new PortalClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForJobStatusChangeResponse())
                };

                object expectedResponseType = typeof (JobStatusChanged);

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
                Assert.Fail();
            }

            [TestMethod]
            [ExpectedException(typeof (UnexpectedResponseException))]
            public async Task ThrowsUnexpectedExceptionWithErrorClassOnUnexpectedError()
            {
                //Arrange
                var portalClient = new PortalClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForErrorResponse())
                };

                //Act
                await portalClient.GetStatusChange();

                //Assert
                Assert.Fail();
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
                Assert.Fail();
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
                Assert.Fail();
            }
        }

        [TestClass]
        public class ConfirmMethod : PortalClientTests
        {
            [TestMethod]
            [ExpectedException(typeof (UnexpectedResponseException), AllowDerivedTypes = true)]
            public async Task ThrowsUnexpectedResponseException()
            {
                //Arrange
                var portalClient = new PortalClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForInternalServerErrorResponse())
                };

                //Act
                await portalClient.Confirm(new ConfirmationReference(new Uri("http://cancellationuri.no")));

                //Assert
                Assert.Fail();
            }
        }
    }
}