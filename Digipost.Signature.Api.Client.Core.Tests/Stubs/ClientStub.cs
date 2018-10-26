using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    internal class ClientStub : BaseClient
    {
        public ClientStub(ClientConfiguration clientConfiguration)
            : this(clientConfiguration, new NullLoggerFactory())
        {
        }

        private ClientStub(ClientConfiguration clientConfiguration, ILoggerFactory loggerFactory)
            : base(clientConfiguration, loggerFactory)
        {
        }

        public Sender GetCurrentSender(Sender jobSender)
        {
            return CurrentSender(jobSender);
        }
    }
}
