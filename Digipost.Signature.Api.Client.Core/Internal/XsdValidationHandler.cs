using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Xsd;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    public class XsdValidationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content != null)
            {
                await ValidateOutgoingAndThrowIfInvalid(request, cancellationToken);
            }
            
            var result = await base.SendAsync(request, cancellationToken);

            await ValidateIncomingAndThrowIfInvalid(result, cancellationToken);

            return result;
        }

        private async Task ValidateIncomingAndThrowIfInvalid(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
        {
            var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

            ValidateBody(responseBody);
        }

        private static async Task ValidateOutgoingAndThrowIfInvalid(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var multipart = await request.Content.ReadAsMultipartAsync(cancellationToken);
            var requestBody = await multipart.Contents.ElementAt(0).ReadAsStringAsync();

            ValidateBody(requestBody);
        }

        private static void ValidateBody(string responseBody)
        {
            var xsdValidator = new XsdValidator();

            xsdValidator.ValiderDokumentMotXsd(responseBody);
            if (!string.IsNullOrEmpty(xsdValidator.ValideringsVarsler))
            {
                throw new InvalidXmlException(xsdValidator.ValideringsVarsler);
            }
        }
    }
}