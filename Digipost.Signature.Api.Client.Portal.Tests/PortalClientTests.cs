﻿using System;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Portal.Enums;
using Digipost.Signature.Api.Client.Portal.Exceptions;
using Digipost.Signature.Api.Client.Portal.Tests.Fakes;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;
using Xunit;
using static Digipost.Signature.Api.Client.Core.Tests.Utilities.CoreDomainUtility;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class PortalClientTests
    {
        public class CreateMethod : PortalClientTests
        {
            [Fact]
            public async Task Throws_exception_on_no_sender()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, GetPostenTestCertificate());
                var portalClient = new PortalClient(clientConfiguration);
                var portalJob = new Job(DomainUtility.GetPortalDocument(), DomainUtility.GetSigners(1), "SendersReference");

                //Act
                await Assert.ThrowsAsync<SenderNotSpecifiedException>(async () => await portalClient.Create(portalJob).ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        public class GetStatusChangeMethod : PortalClientTests
        {
            [Fact]
            public async Task Called_with_both_senders_uses_parameter_sender()
            {
                //Arrange
                var parameterSender = new Sender(PostenOrganizationNumber);
                var clientConfigurationSender = new Sender(BringPublicOrganizationNumber);
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, GetPostenTestCertificate(), clientConfigurationSender);
                var fakeHttpClientHandlerChecksCorrectSender = new FakeHttpClientHandlerChecksCorrectSenderResponse();
                var portalClient = new PortalClient(clientConfiguration)
                {
                    HttpClient = GetHttpClientWithHandler(fakeHttpClientHandlerChecksCorrectSender)
                };

                //Act
                await portalClient.GetStatusChange(parameterSender).ConfigureAwait(false);

                //Assert
                Assert.Contains(parameterSender.OrganizationNumber, fakeHttpClientHandlerChecksCorrectSender.RequestUri);
            }

            [Fact]
            public async Task Can_be_called_without_sender_uses_sender_in_client_configuration()
            {
                //Arrange
                var sender = new Sender(BringPublicOrganizationNumber);
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, GetBringCertificate(), sender);
                var fakeHttpClientHandlerChecksCorrectSender = new FakeHttpClientHandlerChecksCorrectSenderResponse();
                var portalClient = new PortalClient(clientConfiguration)
                {
                    HttpClient = GetHttpClientWithHandler(fakeHttpClientHandlerChecksCorrectSender)
                };

                //Act
                await portalClient.GetStatusChange().ConfigureAwait(false);

                //Assert
                Assert.Contains(sender.OrganizationNumber, fakeHttpClientHandlerChecksCorrectSender.RequestUri);
            }

            [Fact]
            public async Task Returns_empty_object_on_empty_queue()
            {
                //Arrange
                var portalClient = new PortalClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForEmptyQueueResponse())
                };

                //Act
                var actualResponse = await portalClient.GetStatusChange().ConfigureAwait(false);

                //Assert
                Assert.Equal(JobStatus.NoChanges, actualResponse.Status);
                AssertIsWithinTimeSpan(actualResponse.NextPermittedPollTime, TimeSpan.FromSeconds(29), TimeSpan.FromSeconds(31));
            }

            [Fact]
            public async Task Returns_portal_job_status_change_on_ok_response()
            {
                //Arrange
                var portalClient = new PortalClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForJobStatusChangeResponse())
                };

                object expectedResponseType = typeof(JobStatusChanged);

                //Act
                var actualResponse = (await portalClient.GetStatusChange().ConfigureAwait(false));

                //Assert
                Assert.Equal(expectedResponseType, actualResponse.GetType());
                AssertIsWithinTimeSpan(actualResponse.NextPermittedPollTime, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            }

            [Fact]
            public async Task Throws_exception_on_sender_not_specified()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, GetPostenTestCertificate());
                var fakeHttpClientHandlerChecksCorrectSender = new FakeHttpClientHandlerForJobStatusChangeResponse();
                var portalClient = new PortalClient(clientConfiguration)
                {
                    HttpClient = GetHttpClientWithHandler(fakeHttpClientHandlerChecksCorrectSender)
                };

                //Act
                await Assert.ThrowsAsync<SenderNotSpecifiedException>(async () => await portalClient.GetStatusChange().ConfigureAwait(false)).ConfigureAwait(false);
            }

            [Fact]
            public async Task Throws_exception_on_too_many_requests()
            {
                //Arrange
                var portalClient = new PortalClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForTooManyRequestsResponse())
                };

                //Act
                await Assert.ThrowsAsync<TooEagerPollingException>(async () => await portalClient.GetStatusChange().ConfigureAwait(false)).ConfigureAwait(false);
            }

            [Fact]
            public async Task Throws_unexpected_exception_with_error_class_on_unexpected_error()
            {
                //Arrange
                var portalClient = new PortalClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForErrorResponse())
                };

                //Act
                await Assert.ThrowsAsync<UnexpectedResponseException>(async () => await portalClient.GetStatusChange().ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        public class CancelMethod : PortalClientTests
        {
            [Fact]
            public async Task Throws_job_completed_exception_on_conflict()
            {
                //Arrange
                var portalClient = new PortalClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForJobCompletedResponse())
                };

                //Act
                await Assert.ThrowsAsync<JobCompletedException>(async () => await portalClient.Cancel(new CancellationReference(new Uri("http://cancellationuri.no"))).ConfigureAwait(false)).ConfigureAwait(false);
            }

            [Fact]
            public async Task Throws_unexpected_error_on_unexpected_error_code()
            {
                //Arrange
                var portalClient = new PortalClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForInternalServerErrorResponse())
                };

                //Act
                await Assert.ThrowsAsync<UnexpectedResponseException>(async () => await portalClient.Cancel(new CancellationReference(new Uri("http://cancellationuri.no"))).ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        public class ConfirmMethod : PortalClientTests
        {
            [Fact]
            public async Task Throws_unexpected_response_exception()
            {
                //Arrange
                var portalClient = new PortalClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForInternalServerErrorResponse())
                };

                //Act
                await Assert.ThrowsAsync<UnexpectedResponseException>(async () => await portalClient.Confirm(new ConfirmationReference(new Uri("http://cancellationuri.no"))).ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        private static void AssertIsWithinTimeSpan(DateTime actual, TimeSpan spanBeforeActual, TimeSpan spanAfterActual)
        {
            var actualUtc = actual.ToUniversalTime();
            var minDateTimeValueUtc = DateTime.Now.Subtract(spanBeforeActual).ToUniversalTime();
            var maxDateTimeValueUtc = DateTime.Now.Add(spanAfterActual).ToUniversalTime();

            Assert.True(actualUtc > minDateTimeValueUtc, $"The time supplied ({actualUtc:O}) was not after {minDateTimeValueUtc:O}");
            Assert.True(actualUtc < maxDateTimeValueUtc, $"The time supplied ({actualUtc:O}) was not before {maxDateTimeValueUtc:O}");
        }
    }
}
