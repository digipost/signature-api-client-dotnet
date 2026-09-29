using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    /// <summary>
    ///     Acquires and caches OAuth 2.0 access tokens from mIdP using the <em>client credentials</em> grant, where
    ///     the client authenticates to the token endpoint by presenting its certificate during the TLS handshake.
    ///     Tokens are cached in memory and refreshed lazily: a token is considered stale <see cref="RefreshMargin" />
    ///     before its actual expiry, and the next call to <see cref="GetTokenAsync" /> after that point acquires a
    ///     new one. There is no background refresh.
    /// </summary>
    internal class TokenProvider
    {
        private static readonly TimeSpan RefreshMargin = TimeSpan.FromSeconds(30);

        /// <summary>
        ///     A floor under how soon a newly-cached token can be considered stale again, regardless of the
        ///     refresh margin. Without this, a token with a lifetime shorter than <see cref="RefreshMargin" />
        ///     would compute a refresh time already in the past, making every single call to
        ///     <see cref="GetTokenAsync" /> refetch from mIdP - defeating caching entirely and putting
        ///     considerable load on the token endpoint.
        /// </summary>
        private static readonly TimeSpan MinimumCacheTime = TimeSpan.FromSeconds(5);

        private const string AccessTokenScopePrefix = "signering:";

        private readonly ClientConfiguration _clientConfiguration;
        private readonly HttpClient _tokenClient;
        private readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);

        private volatile CachedToken _cachedToken;

        public TokenProvider(ClientConfiguration clientConfiguration)
            : this(clientConfiguration, CreateTokenClient(clientConfiguration))
        {
        }

        internal TokenProvider(ClientConfiguration clientConfiguration, HttpClient tokenClient)
        {
            _clientConfiguration = clientConfiguration;
            _tokenClient = tokenClient;
        }

        private Uri TokenEndpoint
        {
            get
            {
                var tokenEndpoint = _clientConfiguration.TokenEndpoint ?? _clientConfiguration.Environment.TokenEndpoint;

                if (tokenEndpoint == null)
                {
                    throw new ConfigurationException(
                        $"No mIdP token endpoint to acquire access tokens from. The configured {nameof(Environment)} does not have " +
                        $"one, which is expected for custom or local environments. Specify it with {nameof(ClientConfiguration)}.{nameof(ClientConfiguration.TokenEndpoint)}."
                    );
                }

                return tokenEndpoint;
            }
        }

        public void Invalidate(string rejectedAccessToken)
        {
            var current = _cachedToken;
            if (current != null && current.AccessToken == rejectedAccessToken)
            {
                _cachedToken = null;
            }
        }

        public async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            var cached = _cachedToken;
            if (cached != null && !cached.IsStaleAt(DateTimeOffset.UtcNow))
            {
                return cached.AccessToken;
            }

            await _refreshLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                cached = _cachedToken;
                if (cached != null && !cached.IsStaleAt(DateTimeOffset.UtcNow))
                {
                    return cached.AccessToken;
                }

                var refreshed = await AcquireTokenAsync(cancellationToken).ConfigureAwait(false);
                _cachedToken = refreshed;

                return refreshed.AccessToken;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private async Task<CachedToken> AcquireTokenAsync(CancellationToken cancellationToken)
        {
            var tokenEndpoint = TokenEndpoint;

            var requestContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _clientConfiguration.ClientId),
                new KeyValuePair<string, string>("scope", AccessTokenScopePrefix + _clientConfiguration.AccountId.Value),
                // mIdP matches "resource" as an exact string without a trailing slash. Uri.AbsoluteUri always
                // renders a trailing "/" for a bare-authority URL (no path segments), even when the literal
                // Environment.Url has none, so it must be trimmed here.
                new KeyValuePair<string, string>("resource", _clientConfiguration.Environment.Url.AbsoluteUri.TrimEnd('/'))
            });

            using (var response = await _tokenClient.PostAsync(tokenEndpoint, requestContent, cancellationToken).ConfigureAwait(false))
            {
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new AccessTokenException(
                        $"Got {(int) response.StatusCode} {response.StatusCode} from the mIdP token endpoint {tokenEndpoint}, " +
                        $"expected a successful response. The response body was: {Truncate(responseBody)}"
                    );
                }

                return ParseTokenResponse(tokenEndpoint, responseBody, DateTimeOffset.UtcNow);
            }
        }

        internal static CachedToken ParseTokenResponse(Uri tokenEndpoint, string responseBody, DateTimeOffset now)
        {
            JObject tokenResponse;
            try
            {
                tokenResponse = JObject.Parse(responseBody);
            }
            catch (JsonException exception)
            {
                throw new AccessTokenException(
                    $"Could not parse the response from the mIdP token endpoint {tokenEndpoint} as JSON: {Truncate(responseBody)}", exception
                );
            }

            var accessToken = tokenResponse["access_token"];
            if (accessToken == null || accessToken.Type != JTokenType.String || string.IsNullOrEmpty(accessToken.Value<string>()))
            {
                throw new AccessTokenException(
                    $"The response from the mIdP token endpoint {tokenEndpoint} did not contain a non-empty 'access_token' string field. The response body was: {Truncate(responseBody)}"
                );
            }

            var expiresIn = tokenResponse["expires_in"];
            if (expiresIn == null || expiresIn.Type != JTokenType.Integer)
            {
                throw new AccessTokenException(
                    $"The response from the mIdP token endpoint {tokenEndpoint} did not contain an 'expires_in' integer field, so it is not known how long the access token is valid. The response body was: {Truncate(responseBody)}"
                );
            }

            var expiresInSeconds = expiresIn.Value<long>();
            if (expiresInSeconds <= 0)
            {
                throw new AccessTokenException(
                    $"The response from the mIdP token endpoint {tokenEndpoint} stated a lifetime of {expiresInSeconds} seconds for the access token, which is not a usable value."
                );
            }

            var expiresAtUtc = now.AddSeconds(expiresInSeconds);
            var refreshAtUtc = ResolveRefreshAt(now, expiresAtUtc);

            return new CachedToken(accessToken.Value<string>(), expiresAtUtc, refreshAtUtc);
        }

        /// <summary>
        ///     Resolves the point in time at which a cached token should be treated as stale: normally
        ///     <see cref="RefreshMargin" /> before its actual expiry, but never sooner than
        ///     <see cref="MinimumCacheTime" /> from now, and never later than the token's actual expiry.
        /// </summary>
        internal static DateTimeOffset ResolveRefreshAt(DateTimeOffset now, DateTimeOffset expiresAtUtc)
        {
            var refreshAt = expiresAtUtc - RefreshMargin;
            var minimum = now + MinimumCacheTime;

            if (refreshAt > minimum)
            {
                return refreshAt;
            }

            return minimum < expiresAtUtc ? minimum : expiresAtUtc;
        }

        private static string Truncate(string body)
        {
            const int maxLength = 512;

            if (string.IsNullOrEmpty(body))
            {
                return "(empty)";
            }

            return body.Length <= maxLength ? body : body.Substring(0, maxLength) + "... (truncated)";
        }

        private static HttpClient CreateTokenClient(ClientConfiguration clientConfiguration)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(clientConfiguration.Certificate);

            // No ServerCertificateCustomValidationCallback here: mIdP is a separate service from the Signature API,
            // with an ordinary web-PKI certificate, so it is validated using .NET's default TLS validation (system
            // trust store + hostname verification) rather than the org-number-pinned validator used for the API server.
            var proxy = clientConfiguration.WebProxy;
            if (proxy != null)
            {
                proxy.Credentials = clientConfiguration.Credential;
                handler.Proxy = proxy;
                handler.UseProxy = true;
                handler.UseDefaultCredentials = false;
            }

            return new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMilliseconds(clientConfiguration.HttpClientTimeoutInMilliseconds)
            };
        }

        internal sealed class CachedToken
        {
            public CachedToken(string accessToken, DateTimeOffset expiresAtUtc, DateTimeOffset refreshAtUtc)
            {
                AccessToken = accessToken;
                ExpiresAtUtc = expiresAtUtc;
                RefreshAtUtc = refreshAtUtc;
            }

            public string AccessToken { get; }

            /// <summary>
            ///     The token's actual expiry, as stated by mIdP.
            /// </summary>
            public DateTimeOffset ExpiresAtUtc { get; }

            /// <summary>
            ///     The point in time this token should be treated as stale and refreshed - see
            ///     <see cref="TokenProvider.ResolveRefreshAt" />.
            /// </summary>
            public DateTimeOffset RefreshAtUtc { get; }

            public bool IsStaleAt(DateTimeOffset time)
            {
                return time >= RefreshAtUtc;
            }
        }
    }
}
