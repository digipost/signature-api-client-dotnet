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

            string validationWarnings;
            xsdValidator.Validate(xml, out validationWarnings);

            if (!string.IsNullOrEmpty(validationWarnings))
            {
                throw new InvalidXmlException(validationWarnings);
            }
        }
    }
}
