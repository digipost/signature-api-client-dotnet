using System;
using System.Net;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class UnexpectedResponseException : SignatureException
    {
        public UnexpectedResponseException(string message, HttpStatusCode statusCode, Exception innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public UnexpectedResponseException(Error error, HttpStatusCode statusCode)
            : base($"ErrorType:{error.Type}, ErrorCode: {error.Code}, ErrorMessage: {error.Message}")
        {
            Error = error;
            StatusCode = statusCode;
        }

        public Error Error { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
