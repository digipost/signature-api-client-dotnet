using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Direct.Internal;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectClient : BaseClient
    {
        private static readonly Uri DirectJobSubPath = new Uri("/api/signature-jobs", UriKind.Relative);

        public DirectClient(ClientConfiguration clientConfiguration)
            :base(clientConfiguration)
        {
        }

        public async Task<DirectJobResponse> Create(DirectJob directJob)
        {
            var signers = new List<Signer> {directJob.Signer};
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, directJob.Document, signers, ClientConfiguration.Certificate);
            var createAction = new CreateAction(directJob, documentBundle, ClientConfiguration.Certificate, ClientConfiguration.SignatureServiceRoot);
            var requestResult = await createAction.PostAsync(HttpClient, DirectJobSubPath);
            
            return CreateAction.DeserializeFunc(await requestResult.Content.ReadAsStringAsync());
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
