using System;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class TooEagerPollingException : SignatureException
    {
        public TooEagerPollingException(string message)
            : base($"Excessive polling is not allowed. The the next permitted poll time is: {message}, and can also be found in variable 'NextPermittedPollTime' on exception class.")
        {
            NextPermittedPollTime = DateTime.Parse(message);
        }

        public DateTime NextPermittedPollTime { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, NextPermittedPollTime: {NextPermittedPollTime}";
        }
    }
}