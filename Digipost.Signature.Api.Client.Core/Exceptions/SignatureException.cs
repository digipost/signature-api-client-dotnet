using System;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class SignatureException : Exception
    {
        public SignatureException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SignatureException(string message)
            : base(message)
        {
        }

        public SignatureException()
        {
        }
    }
}
