using System;
using Digipost.Signature.Api.Client.Core.Exceptions;

namespace Digipost.Signature.Api.Client.Portal
{
    public class TooEagerPollingException : SignatureException
    {
        public TooEagerPollingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public TooEagerPollingException(string message)
            : base(message)
        {
            NextPermittedPollTime = DateTime.Parse(message);
        }

        public TooEagerPollingException()
        {
        }

        public DateTime NextPermittedPollTime { get; set; }
    }
}