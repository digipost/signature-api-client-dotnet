using System;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class XmlParseException : ConfigurationException
    {
        public XmlParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public XmlParseException(string message)
            : base(message)
        {
        }

        public XmlParseException()
        {
        }
    }
}