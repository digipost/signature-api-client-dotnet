using System;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class TooEagerPollingException : SignatureException
    {

        public TooEagerPollingException(string nextPermittedPollTime)
            : this(DateTime.Parse(nextPermittedPollTime))
        {
        }
        
        public TooEagerPollingException(DateTime nextPermittedPollTime)
            : base($"Excessive polling is not allowed. The next permitted poll time is: {nextPermittedPollTime.ToUniversalTime().ToString("O")}, and can also be found in variable 'NextPermittedPollTime' on exception class.")
        {
            NextPermittedPollTime = nextPermittedPollTime;
        }
        
        public DateTime NextPermittedPollTime { get; }

        public override string ToString()
        {
            return $"{base.ToString()}, NextPermittedPollTime: {NextPermittedPollTime}";
        }
    }
}
