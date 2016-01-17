using System;
using System.Collections.Generic;
using System.IO;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;

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
            var serializedjob = DataTransferObjectConverter.ToDataTransferObject(directJob);

            var signers = new List<Signer> {directJob.Signer};
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, directJob.Document, signers, ClientConfiguration.Certificate);

            //Todo: pack and send.
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
