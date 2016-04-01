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
            
            return await base.SendAsync(request, cancellationToken);
        }

        private static async Task ValidateOutgoingAndThrowIfInvalid(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var xsdValidator = new XsdValidator();

            var multipart = await request.Content.ReadAsMultipartAsync(cancellationToken);
            var bodyPartXml = await multipart.Contents.ElementAt(0).ReadAsStringAsync();

            try
            {
                xsdValidator.ValiderDokumentMotXsd(bodyPartXml);
            }
            catch (XmlException)
            {
                throw new InvalidXmlException($"Invalid outgoing Xml: {xsdValidator.ValideringsVarsler}");
            }
        }
    }
}