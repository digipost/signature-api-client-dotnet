using System;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class TooEagerPollingException : SignatureException
    {
        public TooEagerPollingException(string nextPermittedPollTime)
            : base($"Excessive polling is not allowed. The next permitted poll time is: {nextPermittedPollTime}, and can also be found in variable 'NextPermittedPollTime' on exception class.")
        {
            NextPermittedPollTime = DateTime.Parse(nextPermittedPollTime);
        }

        public DateTime NextPermittedPollTime { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, NextPermittedPollTime: {NextPermittedPollTime}";
        }
    }
}