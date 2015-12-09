using System;
using System.IO;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectClient
    {
        private ClientHelper _clientHelper;
        public ClientConfiguration ClientConfiguration { get; }

        public DirectClient(ClientConfiguration clientConfiguration)
        {
            ClientConfiguration = clientConfiguration;
            _clientHelper = new ClientHelper();
        }

        public DirectJobResponse Create(DirectJob directJob)
        {
            throw new NotImplementedException();
        }

        public DirectJobStatusResponse GetStatus(DirectJobReference directJobReference)
        {
            throw new NotImplementedException();
        }

        public Stream GetXades(XadesReference xadesReference)
        {
            throw new NotImplementedException();
        }

        public Stream GetPades(PadesReference xadesReference)
        {
            throw new NotImplementedException();
        }

    }
}
