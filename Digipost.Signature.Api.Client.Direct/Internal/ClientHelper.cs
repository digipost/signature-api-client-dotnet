using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct.Internal
{
    public class ClientHelper
    {
        private static readonly Uri DirectJobSubPath = new Uri("/api/signature-jobs", UriKind.Relative);

        public ClientConfiguration ClientConfiguration { get; }

        public ClientHelper(ClientConfiguration clientConfiguration)
        {
            ClientConfiguration = clientConfiguration;
        }
        
        public async Task<HttpResponseMessage> SendDirectJobRequest(DirectJob directJob, DocumentBundle documentBundle)
        {
            var createAction = new CreateAction(directJob, documentBundle, ClientConfiguration.Certificate, ClientConfiguration.SignatureServiceRoot);
            return await createAction.PostAsync(DirectJobSubPath);
        } 
    }
}
