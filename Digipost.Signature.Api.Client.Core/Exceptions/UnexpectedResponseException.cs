using System;
using System.Net;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class UnexpectedResponseException : SignatureException
    {
        public Error Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public UnexpectedResponseException(string message, HttpStatusCode statusCode, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public UnexpectedResponseException(Error error, HttpStatusCode statusCode) : base(string.Format("ErrorType:{0}, ErrorCode: {1}, ErrorMessage: {2}", error.Type, error.Code, error.Message))
        {
            Error = error;
            StatusCode = statusCode;
        }
    }
}
