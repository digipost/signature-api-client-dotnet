using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal class XsdRequestValidationHandler : XsdValidationHandler
    {
        private const string MultipartMixed = "multipart/mixed";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await ValidateXmlAndThrowIfInvalid(request.Content, cancellationToken);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task ValidateXmlAndThrowIfInvalid(HttpContent content, CancellationToken cancellationToken)
        {
            var contentMediaType = content?.Headers.ContentType?.MediaType;

            if (contentMediaType == MultipartMixed)
            {
                var multipart = await content.ReadAsMultipartAsync(cancellationToken);

                foreach (var httpContent in multipart.Contents.Where(httpContent => httpContent.Headers.ContentType.MediaType == ApplicationXml))
                {
                    ValidateXml(await httpContent.ReadAsStringAsync());
                }
            }
        }
    }
}