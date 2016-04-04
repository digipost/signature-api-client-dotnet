using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Difi.Felles.Utility;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Xsd;
using log4net;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    public class XsdValidationHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await ValidateXmlAndThrowIfInvalid(request.Content, cancellationToken);
            
            var result = await base.SendAsync(request, cancellationToken);

            await ValidateXmlAndThrowIfInvalid(result.Content, cancellationToken);

            return result;
        }

        private static async Task ValidateXmlAndThrowIfInvalid(HttpContent content, CancellationToken cancellationToken)
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