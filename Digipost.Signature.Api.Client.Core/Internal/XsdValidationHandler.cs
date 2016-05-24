using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal.Xsd;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    public class XsdValidationHandler : DelegatingHandler
    {
        protected static void ValidateXmlAndThrowIfInvalid(string xml)
        {
            var xsdValidator = new XsdValidator();
            xsdValidator.Validate(xml);

            if (!string.IsNullOrEmpty(xsdValidator.ValidationWarnings))
            {
                throw new InvalidXmlException(xsdValidator.ValidationWarnings);
            }
        }
    }
}