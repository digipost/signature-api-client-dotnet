using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal class XsdResponseValidationHandler : XsdValidationHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = await base.SendAsync(request, cancellationToken);

            var requestUri = request.RequestUri.AbsolutePath.ToLower();
            var mustValidateForCurrentEndpoint = !requestUri.Contains("xades") && !requestUri.Contains("pades");
            if (mustValidateForCurrentEndpoint)
            {
                await ValidateAndThrowIfInvalid(result.Content);
            }

            return result;
        }

        private async Task ValidateAndThrowIfInvalid(HttpContent content)
        {
            var contentMediaType = content?.Headers.ContentType?.MediaType;

            if (contentMediaType == ApplicationXml)
            {
                var readAsStringAsync = await content.ReadAsStringAsync();
                ValidateXmlAndThrowIfInvalid(readAsStringAsync);
            }
        }
    }
}