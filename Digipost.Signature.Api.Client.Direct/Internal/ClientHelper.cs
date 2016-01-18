using System;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct.Internal
{
    public class ClientHelper
    {
        public ClientConfiguration ClientConfiguration { get; }

        public ClientHelper(ClientConfiguration clientConfiguration)
        {
            ClientConfiguration = clientConfiguration;
        }

        public DirectJobResponse SendDirectJobRequest(DirectJob directJob, DocumentBundle documentBundle)
        {
            var createAction = new CreateAction(directJob, documentBundle, ClientConfiguration.Certificate, ClientConfiguration.SignatureServiceRoot);
            createAction.PostAsync();

            return null;
        } 
    }
}
