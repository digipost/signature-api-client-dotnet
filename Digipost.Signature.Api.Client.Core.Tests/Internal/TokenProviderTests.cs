using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Internal
{
    public class TokenProviderTests
    {
        private static ClientConfiguration GetClientConfiguration(Environment environment, Uri tokenEndpoint = null)
        {
            return new ClientConfiguration(environment, CoreDomainUtility.GetTestCertificate(), CoreDomainUtility.JwtClientId, CoreDomainUtility.GetAccountId(), CoreDomainUtility.GetSender())
            {
                TokenEndpoint = tokenEndpoint
            };
        }

        private class QueuedResponsesHandler : HttpMessageHandler
        {
            private readonly Queue<HttpResponseMessage> _responses;

            public int Calls { get; private set; }

            public QueuedResponsesHandler(params HttpResponseMessage[] responses)
            {
                _responses = new Queue<HttpResponseMessage>(responses);
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Calls++;
                return Task.FromResult(_responses.Dequeue());
            }
        }

        private static HttpResponseMessage TokenResponse(string body, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new HttpResponseMessage(statusCode) {Content = new StringContent(body)};
        }

        private class CapturingHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;

            public string CapturedRequestBody { get; private set; }

            public CapturingHandler(HttpResponseMessage response)
            {
                _response = response;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                CapturedRequestBody = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                return _response;
            }
        }

        private static Dictionary<string, string> ParseFormBody(string body)
        {
            var result = new Dictionary<string, string>();
            foreach (var pair in body.Split('&'))
            {
                var parts = pair.Split('=');
                result[Uri.UnescapeDataString(parts[0])] = Uri.UnescapeDataString(parts[1]);
            }

            return result;
        }

        public class ParseTokenResponseMethod : TokenProviderTests
        {
            [Fact]
            public void Parses_access_token_and_expiry()
            {
                //Arrange
                var now = DateTimeOffset.UtcNow;
                var body = "{\"access_token\": \"the-token\", \"expires_in\": 60}";

                //Act
                var cachedToken = TokenProvider.ParseTokenResponse(new Uri("https://midp.example.no/oauth2/token"), body, now);

                //Assert
                Assert.Equal("the-token", cachedToken.AccessToken);
                Assert.Equal(now.AddSeconds(60), cachedToken.ExpiresAtUtc);
            }

            [Fact]
            public void Throws_when_access_token_is_missing()
            {
                //Arrange
                var body = "{\"expires_in\": 60}";

                //Act
                //Assert
                Assert.Throws<AccessTokenException>(() => TokenProvider.ParseTokenResponse(new Uri("https://midp.example.no/oauth2/token"), body, DateTimeOffset.UtcNow));
            }

            [Fact]
            public void Throws_when_expires_in_is_missing()
            {
                //Arrange
                var body = "{\"access_token\": \"the-token\"}";

                //Act
                //Assert
                Assert.Throws<AccessTokenException>(() => TokenProvider.ParseTokenResponse(new Uri("https://midp.example.no/oauth2/token"), body, DateTimeOffset.UtcNow));
            }

            [Fact]
            public void Throws_when_expires_in_is_not_positive()
            {
                //Arrange
                var body = "{\"access_token\": \"the-token\", \"expires_in\": 0}";

                //Act
                //Assert
                Assert.Throws<AccessTokenException>(() => TokenProvider.ParseTokenResponse(new Uri("https://midp.example.no/oauth2/token"), body, DateTimeOffset.UtcNow));
            }

            [Fact]
            public void Throws_when_body_is_not_json()
            {
                //Arrange
                var body = "<html>not json</html>";

                //Act
                //Assert
                Assert.Throws<AccessTokenException>(() => TokenProvider.ParseTokenResponse(new Uri("https://midp.example.no/oauth2/token"), body, DateTimeOffset.UtcNow));
            }

            [Fact]
            public void Applies_minimum_cache_time_floor_for_short_lived_token()
            {
                // A 20 second lifetime minus the 30 second refresh margin would otherwise resolve to a
                // refresh time already in the past - see docs/adr and TokenProvider.ResolveRefreshAt.
                //Arrange
                var now = DateTimeOffset.UtcNow;
                var body = "{\"access_token\": \"the-token\", \"expires_in\": 20}";

                //Act
                var cachedToken = TokenProvider.ParseTokenResponse(new Uri("https://midp.example.no/oauth2/token"), body, now);

                //Assert
                Assert.Equal(now.AddSeconds(20), cachedToken.ExpiresAtUtc);
                Assert.Equal(now.AddSeconds(5), cachedToken.RefreshAtUtc);
                Assert.False(cachedToken.IsStaleAt(now));
            }
        }

        public class ResolveRefreshAtMethod : TokenProviderTests
        {
            [Fact]
            public void Uses_refresh_margin_for_normal_lifetime()
            {
                //Arrange
                var now = DateTimeOffset.UtcNow;
                var expiresAtUtc = now.AddSeconds(3600);

                //Act
                var refreshAt = TokenProvider.ResolveRefreshAt(now, expiresAtUtc);

                //Assert
                Assert.Equal(expiresAtUtc.AddSeconds(-30), refreshAt);
            }

            [Fact]
            public void Floors_to_minimum_cache_time_for_short_lifetime()
            {
                //Arrange
                var now = DateTimeOffset.UtcNow;
                var expiresAtUtc = now.AddSeconds(20);

                //Act
                var refreshAt = TokenProvider.ResolveRefreshAt(now, expiresAtUtc);

                //Assert
                Assert.Equal(now.AddSeconds(5), refreshAt);
            }

            [Fact]
            public void Never_resolves_later_than_actual_expiry_for_pathologically_short_lifetime()
            {
                //Arrange
                var now = DateTimeOffset.UtcNow;
                var expiresAtUtc = now.AddSeconds(2);

                //Act
                var refreshAt = TokenProvider.ResolveRefreshAt(now, expiresAtUtc);

                //Assert
                Assert.Equal(expiresAtUtc, refreshAt);
            }
        }

        public class GetTokenAsyncMethod : TokenProviderTests
        {
            [Fact]
            public async Task Acquires_and_caches_token()
            {
                //Arrange
                var handler = new QueuedResponsesHandler(TokenResponse("{\"access_token\": \"the-token\", \"expires_in\": 3600}"));
                var tokenProvider = new TokenProvider(GetClientConfiguration(Environment.DifiQa), new HttpClient(handler));

                //Act
                var firstToken = await tokenProvider.GetTokenAsync().ConfigureAwait(false);
                var secondToken = await tokenProvider.GetTokenAsync().ConfigureAwait(false);

                //Assert
                Assert.Equal("the-token", firstToken);
                Assert.Equal("the-token", secondToken);
                Assert.Equal(1, handler.Calls);
            }

            [Fact]
            public async Task Acquires_new_token_after_invalidate()
            {
                //Arrange
                var handler = new QueuedResponsesHandler(
                    TokenResponse("{\"access_token\": \"first-token\", \"expires_in\": 3600}"),
                    TokenResponse("{\"access_token\": \"second-token\", \"expires_in\": 3600}")
                );
                var tokenProvider = new TokenProvider(GetClientConfiguration(Environment.DifiQa), new HttpClient(handler));

                //Act
                var firstToken = await tokenProvider.GetTokenAsync().ConfigureAwait(false);
                tokenProvider.Invalidate(firstToken);
                var secondToken = await tokenProvider.GetTokenAsync().ConfigureAwait(false);

                //Assert
                Assert.Equal("first-token", firstToken);
                Assert.Equal("second-token", secondToken);
                Assert.Equal(2, handler.Calls);
            }

            [Fact]
            public async Task Invalidate_with_stale_token_does_not_discard_current_token()
            {
                //Arrange
                var handler = new QueuedResponsesHandler(TokenResponse("{\"access_token\": \"the-token\", \"expires_in\": 3600}"));
                var tokenProvider = new TokenProvider(GetClientConfiguration(Environment.DifiQa), new HttpClient(handler));
                var currentToken = await tokenProvider.GetTokenAsync().ConfigureAwait(false);

                //Act
                tokenProvider.Invalidate("a-different-token-that-was-already-replaced");
                var tokenAfterNoOpInvalidate = await tokenProvider.GetTokenAsync().ConfigureAwait(false);

                //Assert
                Assert.Equal(currentToken, tokenAfterNoOpInvalidate);
                Assert.Equal(1, handler.Calls);
            }

            [Fact]
            public async Task Throws_when_response_is_not_successful()
            {
                //Arrange
                var handler = new QueuedResponsesHandler(TokenResponse("error", HttpStatusCode.BadRequest));
                var tokenProvider = new TokenProvider(GetClientConfiguration(Environment.DifiQa), new HttpClient(handler));

                //Act
                //Assert
                await Assert.ThrowsAsync<AccessTokenException>(async () => await tokenProvider.GetTokenAsync().ConfigureAwait(false)).ConfigureAwait(false);
            }

            [Fact]
            public async Task Throws_configuration_exception_when_no_token_endpoint_is_available()
            {
                //Arrange
                var handler = new QueuedResponsesHandler();
                var tokenProvider = new TokenProvider(GetClientConfiguration(Environment.Localhost), new HttpClient(handler));

                //Act
                //Assert
                await Assert.ThrowsAsync<ConfigurationException>(async () => await tokenProvider.GetTokenAsync().ConfigureAwait(false)).ConfigureAwait(false);
            }

            [Fact]
            public async Task Uses_token_endpoint_override_when_environment_has_none()
            {
                //Arrange
                var handler = new QueuedResponsesHandler(TokenResponse("{\"access_token\": \"the-token\", \"expires_in\": 3600}"));
                var tokenProvider = new TokenProvider(GetClientConfiguration(Environment.Localhost, new Uri("https://localhost:8443/oauth2/token")), new HttpClient(handler));

                //Act
                var token = await tokenProvider.GetTokenAsync().ConfigureAwait(false);

                //Assert
                Assert.Equal("the-token", token);
            }

            [Fact]
            public async Task Sends_resource_without_trailing_slash()
            {
                // Uri.AbsoluteUri always renders a trailing "/" for a bare-authority URL such as
                // Environment.DifiQa.Url, even though the literal has none. mIdP matches "resource" as an
                // exact string and rejects one with a trailing slash as an unknown target.
                //Arrange
                var handler = new CapturingHandler(TokenResponse("{\"access_token\": \"the-token\", \"expires_in\": 3600}"));
                var tokenProvider = new TokenProvider(GetClientConfiguration(Environment.DifiQa), new HttpClient(handler));

                //Act
                await tokenProvider.GetTokenAsync().ConfigureAwait(false);

                //Assert
                var sentParameters = ParseFormBody(handler.CapturedRequestBody);
                Assert.Equal("https://api.difiqa.signering.posten.no", sentParameters["resource"]);
            }

            [Fact]
            public async Task Sends_scope_with_signering_prefix_and_account_id()
            {
                //Arrange
                var handler = new CapturingHandler(TokenResponse("{\"access_token\": \"the-token\", \"expires_in\": 3600}"));
                var tokenProvider = new TokenProvider(GetClientConfiguration(Environment.DifiQa), new HttpClient(handler));

                //Act
                await tokenProvider.GetTokenAsync().ConfigureAwait(false);

                //Assert
                var sentParameters = ParseFormBody(handler.CapturedRequestBody);
                Assert.Equal("signering:123456", sentParameters["scope"]);
            }
        }
    }
}
