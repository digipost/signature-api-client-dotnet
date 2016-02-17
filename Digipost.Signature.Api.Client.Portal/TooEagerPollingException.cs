using System;
using Digipost.Signature.Api.Client.Core.Exceptions;

namespace Digipost.Signature.Api.Client.Portal
{
    public class TooEagerPollingException : SignatureException
    {
        public DateTime NextPermittedPollTime { get; set; }

        public TooEagerPollingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public TooEagerPollingException(string message) : base(message)
        {
            NextPermittedPollTime = DateTime.Parse(message);
        }                                                                                                                                               

        public TooEagerPollingException()
        {
        }


    }
}
