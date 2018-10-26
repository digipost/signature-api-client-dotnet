using System;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class CertificateException : SecurityException
    {
        public CertificateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
