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

            var mustValidateForCurrentEndpoint = !request.RequestUri.AbsolutePath.ToLower().Contains("xades");
            if (mustValidateForCurrentEndpoint)
            {
                await ValidateXmlAndThrowIfInvalid(result.Content);
            }

            return result;
        }

        private async Task ValidateXmlAndThrowIfInvalid(HttpContent content)
        {
            var contentMediaType = content?.Headers.ContentType?.MediaType;

            if (contentMediaType == ApplicationXml)
            {
                var readAsStringAsync = await content.ReadAsStringAsync();
                ValidateXml(readAsStringAsync);
            }
        }
    }
}