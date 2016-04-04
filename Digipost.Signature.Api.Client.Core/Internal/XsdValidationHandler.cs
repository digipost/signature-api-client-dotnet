using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Xsd;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    public class XsdValidationHandler : DelegatingHandler
    {
        protected static async Task ValidateXmlAndThrowIfInvalid(HttpContent content, CancellationToken cancellationToken)
        {
            if (content?.Headers.ContentType?.MediaType == "multipart/mixed")
            {
                var multipart = await content.ReadAsMultipartAsync(cancellationToken);
                var requestBody = await multipart.Contents.ElementAt(0).ReadAsStringAsync();
                ValidateXmlBody(requestBody);
            }
        }

        private static void ValidateXmlBody(string body)
        {
            var xsdValidator = new XsdValidator();

            xsdValidator.ValiderDokumentMotXsd(body);
            if (!string.IsNullOrEmpty(xsdValidator.ValideringsVarsler))
            {
                throw new InvalidXmlException(xsdValidator.ValideringsVarsler);
            }
        }
    }
}