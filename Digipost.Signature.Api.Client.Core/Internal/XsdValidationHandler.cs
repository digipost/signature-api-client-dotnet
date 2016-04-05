using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Xsd;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    public class XsdValidationHandler : DelegatingHandler
    {
        protected const string ApplicationXml = "application/xml";

        protected static void ValidateXml(string xml)
        {
            var xsdValidator = new XsdValidator();
            xsdValidator.ValiderDokumentMotXsd(xml);

            if (!string.IsNullOrEmpty(xsdValidator.ValideringsVarsler))
            {
                throw new InvalidXmlException(xsdValidator.ValideringsVarsler);
            }
        }
    }
}