using System;
using System.Net;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class BrokerNotAuthorizedException : UnexpectedResponseException
    {
        public BrokerNotAuthorizedException(Error error, HttpStatusCode statusCode) : base(error, statusCode)
        {
        }
    }
}
