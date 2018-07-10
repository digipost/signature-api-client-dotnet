namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    internal class ClientStub : BaseClient
    {
        public ClientStub(ClientConfiguration clientConfiguration)
            : base(clientConfiguration)
        {
        }

        public Sender GetCurrentSender(Sender jobSender)
        {
            return CurrentSender(jobSender);
        }
    }
}