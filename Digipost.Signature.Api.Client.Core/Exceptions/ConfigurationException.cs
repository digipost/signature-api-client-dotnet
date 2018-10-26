using System;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class ConfigurationException : SignatureException
    {
        public ConfigurationException(string message, Exception innerException)
            :
            base(message, innerException)
        {
        }

        public ConfigurationException(string message)
            : base(message)
        {
        }

        public ConfigurationException()
        {
        }
    }
}
