namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    public class SenderNotSpecifiedException : SignatureException
    {
        public SenderNotSpecifiedException()
            : base("A sender must either be specified globally in ClientConfiguration or for each job.")
        {
        }
    }
}