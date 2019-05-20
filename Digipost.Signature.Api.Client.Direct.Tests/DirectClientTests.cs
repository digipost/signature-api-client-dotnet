using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Direct.Tests.Fakes;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;
using static Digipost.Signature.Api.Client.Core.Tests.Utilities.CoreDomainUtility;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class DirectClientTests
    {
        public class ConstructorMethod : DirectClientTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var clientConfiguration = GetClientConfiguration();

                //Act
                var client = new DirectClient(clientConfiguration);

                //Assert
                Assert.Equal(clientConfiguration, client.ClientConfiguration);
                Assert.NotNull(client.HttpClient);
            }
        }

        public class CreateMethod : DirectClientTests
        {
            [Fact]
            public async Task Throws_exception_on_no_sender()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, GetPostenTestCertificate());
                var directClient = new DirectClient(clientConfiguration);
                var directJob = new Job(DomainUtility.GetDirectDocument(), DomainUtility.GetSigner(), "SendersReference", DomainUtility.GetExitUrls());

                //Act
                await Assert.ThrowsAsync<SenderNotSpecifiedException>(async () => await directClient.Create(directJob).ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        public class GetStatusMethod : DirectClientTests
        {
            [Fact]
            public async Task Get_status_throws_unexpected_response_exception_on_server_error()
            {
                //Arrange
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = new HttpClient(new FakeHttpClientHandlerForInternalServerErrorResponse())
                };

                //Act
                await Assert.ThrowsAsync<UnexpectedResponseException>(async () => await directClient.GetStatus(new StatusReference(new Uri("http://statusReference.no"), "StatusQueryToken")).ConfigureAwait(false)).ConfigureAwait(false);
            }

            [Fact]
            public async Task Returns_status_response()
            {
                //Arrange
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = new HttpClient(new FakeHttpClientHandlerGetStatusResponse())
                };

                //Act
                var result = await directClient.GetStatus(new StatusReference(new Uri("http://statusReference.no"), "StatusQueryToken")).ConfigureAwait(false);

                //Assert
                Assert.NotNull(result);
            }
        }

        public class GetStatusChangeMethod : DirectClientTests
        {
            [Fact]
            public async Task Cant_get_signature_job_id_from_empty_response()
            {
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForEmptyQueueResponse())
                };

                var statusChange = await directClient.GetStatusChange().ConfigureAwait(false);
                Assert.Throws<InvalidOperationException>(() => statusChange.JobId);
            }

            [Fact]
            public async Task Returns_empty_object_on_empty_queue()
            {
                var fakeEmptyQueueResponse = new FakeHttpClientHandlerForEmptyQueueResponse();
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(fakeEmptyQueueResponse)
                };

                var actualResponse = await directClient.GetStatusChange().ConfigureAwait(false);

                Assert.Throws<InvalidOperationException>(() => actualResponse.JobId);
                Assert.Null(actualResponse.JobReference);
            }

            [Fact]
            public async Task Returns_status_response()
            {
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerGetStatusResponse())
                };

                var result = await directClient.GetStatusChange().ConfigureAwait(false);

                Assert.NotNull(result);
            }

            [Fact]
            public async Task Throws_too_eager_polling_exception_on_too_many_requests()
            {
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForTooManyRequestsResponse())
                };

                await Assert.ThrowsAsync<TooEagerPollingException>(async () => await directClient.GetStatusChange().ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        public class RequestNewRedirectUrl : DirectClientTests
        {
            [Fact]
            public async Task Can_request_new_redirect_url()
            {
                //Arrange
                var directClient = new DirectClient(GetClientConfiguration())
                {
                    HttpClient = GetHttpClientWithHandler(new FakeHttpClientHandlerForNewRedirectUrlResponse())
                };

                var newRedirectUrl = await directClient.requestNewRedirectUrl();

                Assert.NotNull(newRedirectUrl.Identifier);
                Assert.NotNull(newRedirectUrl.RedirectUrl);
            }
        }
    }
}
