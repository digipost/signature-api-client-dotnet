using System;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class SecurityException : SignatureException
    {
        public SecurityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SecurityException(string message)
            : base(message)
        {
        }
    }
}