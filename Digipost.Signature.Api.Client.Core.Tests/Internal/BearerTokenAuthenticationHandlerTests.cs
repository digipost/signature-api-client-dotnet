using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Internal
{
    public class BearerTokenAuthenticationHandlerTests
    {
        private class RecordingApiHandler : HttpMessageHandler
        {
            private readonly Queue<HttpStatusCode> _statusCodes;

            public List<string> AuthorizationHeadersSeen { get; } = new List<string>();
            public List<string> ContentSeen { get; } = new List<string>();

            public RecordingApiHandler(params HttpStatusCode[] statusCodes)
            {
                _statusCodes = new Queue<HttpStatusCode>(statusCodes);
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                AuthorizationHeadersSeen.Add(request.Headers.Authorization?.ToString());
                ContentSeen.Add(request.Content == null ? null : await request.Content.ReadAsStringAsync().ConfigureAwait(false));

                return new HttpResponseMessage(_statusCodes.Dequeue());
            }
        }

        private class QueuedTokenResponsesHandler : HttpMessageHandler
        {
            private readonly Queue<string> _accessTokens;

            public QueuedTokenResponsesHandler(params string[] accessTokens)
            {
                _accessTokens = new Queue<string>(accessTokens);
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var accessToken = _accessTokens.Dequeue();

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"{{\"access_token\": \"{accessToken}\", \"expires_in\": 3600}}")
                });
            }
        }

        private static HttpClient GetClient(RecordingApiHandler apiHandler, params string[] accessTokens)
        {
            var clientConfiguration = new ClientConfiguration(Environment.DifiTest, CoreDomainUtility.GetTestCertificate(), CoreDomainUtility.JwtClientId, CoreDomainUtility.GetAccountId(), CoreDomainUtility.GetSender());
            var tokenProvider = new TokenProvider(clientConfiguration, new HttpClient(new QueuedTokenResponsesHandler(accessTokens)));

            var bearerTokenHandler = new BearerTokenAuthenticationHandler(tokenProvider) {InnerHandler = apiHandler};

            return new HttpClient(bearerTokenHandler) {BaseAddress = new Uri("https://api.example.no")};
        }

        public class SendAsync : BearerTokenAuthenticationHandlerTests
        {
            [Fact]
            public async Task Attaches_bearer_token_to_request()
            {
                //Arrange
                var apiHandler = new RecordingApiHandler(HttpStatusCode.OK);
                var client = GetClient(apiHandler, "the-token");

                //Act
                await client.GetAsync("signature-jobs").ConfigureAwait(false);

                //Assert
                Assert.Equal("Bearer the-token", apiHandler.AuthorizationHeadersSeen[0]);
            }

            [Fact]
            public async Task Retries_once_with_fresh_token_on_401()
            {
                //Arrange
                var apiHandler = new RecordingApiHandler(HttpStatusCode.Unauthorized, HttpStatusCode.OK);
                var client = GetClient(apiHandler, "stale-token", "fresh-token");

                //Act
                var response = await client.GetAsync("signature-jobs").ConfigureAwait(false);

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(new List<string> {"Bearer stale-token", "Bearer fresh-token"}, apiHandler.AuthorizationHeadersSeen);
            }

            [Fact]
            public async Task Retries_a_post_request_with_body_on_401()
            {
                // Unconditional retry, including non-idempotent requests such as signature job creation -
                // see docs/adr/0003-jwt-401-unconditional-retry.md
                //Arrange
                var apiHandler = new RecordingApiHandler(HttpStatusCode.Unauthorized, HttpStatusCode.OK);
                var client = GetClient(apiHandler, "stale-token", "fresh-token");

                //Act
                var response = await client.PostAsync("signature-jobs", new StringContent("job-body", Encoding.UTF8)).ConfigureAwait(false);

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(new List<string> {"job-body", "job-body"}, apiHandler.ContentSeen);
                Assert.Equal(new List<string> {"Bearer stale-token", "Bearer fresh-token"}, apiHandler.AuthorizationHeadersSeen);
            }

            [Fact]
            public async Task Does_not_retry_more_than_once()
            {
                //Arrange
                var apiHandler = new RecordingApiHandler(HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized);
                var client = GetClient(apiHandler, "token-1", "token-2");

                //Act
                var response = await client.GetAsync("signature-jobs").ConfigureAwait(false);

                //Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                Assert.Equal(2, apiHandler.AuthorizationHeadersSeen.Count);
            }
        }
    }
}
