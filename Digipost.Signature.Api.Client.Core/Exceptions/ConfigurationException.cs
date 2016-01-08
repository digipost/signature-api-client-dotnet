using System;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class ConfigurationException : SignatureException
    {
        protected ConfigurationException(string message, Exception innerException)
        {
            throw new NotImplementedException();
        }

        public ConfigurationException(string message) : base(message)
        {
            
        }

        public ConfigurationException()
        {
            
        }
    }
}
