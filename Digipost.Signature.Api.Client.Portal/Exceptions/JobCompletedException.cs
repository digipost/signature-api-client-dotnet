using Digipost.Signature.Api.Client.Core.Exceptions;

namespace Digipost.Signature.Api.Client.Portal.Exceptions
{
    public class JobCompletedException : SignatureException
    {
        public JobCompletedException()
            : base("The service refused to process the cancellation. This happens when the job has been completed " +
                   "(i.e. all signers have signed, rejected, etc.) since receiving last update. Please ask the service for status changes to get the latest changes.")
        {
        }
    }
}