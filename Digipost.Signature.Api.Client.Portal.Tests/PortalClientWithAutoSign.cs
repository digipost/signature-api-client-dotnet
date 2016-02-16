using System;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class PortalClientWithAutoSign : PortalClient
    {
        public PortalClientWithAutoSign(ClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

    }
}
