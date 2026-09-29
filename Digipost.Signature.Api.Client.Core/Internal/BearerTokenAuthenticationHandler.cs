using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    /// <summary>
    ///     Attaches an <c>Authorization: Bearer</c> header acquired from a <see cref="TokenProvider" /> to every
    ///     request. Also recovers from a token being rejected: a token can stop working before it is considered
    ///     stale by the <see cref="TokenProvider" />, e.g. if it is revoked, or if this host's clock runs ahead of
    ///     mIdP's. On a 401, the rejected token is invalidated and the request is retried exactly once with a
    ///     freshly-acquired token, regardless of HTTP method - see
    ///     docs/adr/0003-jwt-401-unconditional-retry.md for why this includes signature job creation.
    /// </summary>
    internal class BearerTokenAuthenticationHandler : DelegatingHandler
    {
        private readonly TokenProvider _tokenProvider;

        public BearerTokenAuthenticationHandler(TokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            _tokenProvider.Invalidate(accessToken);

            var retryRequest = await CloneRequestAsync(request).ConfigureAwait(false);
            var refreshedToken = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(false);
            retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshedToken);

            response.Dispose();

            return await base.SendAsync(retryRequest, cancellationToken).ConfigureAwait(false);
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri) {Version = request.Version};

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (request.Content != null)
            {
                var contentBytes = await request.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                var clonedContent = new ByteArrayContent(contentBytes);
                foreach (var header in request.Content.Headers)
                {
                    clonedContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                clone.Content = clonedContent;
            }

            return clone;
        }
    }
}
