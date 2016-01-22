using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
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
            var createAction = new CreateAction(directJob, documentBundle);
            var requestResult = await HttpClient.PostAsync(DirectJobSubPath, createAction.Content());
            
            return CreateAction.DeserializeFunc(await requestResult.Content.ReadAsStringAsync());
        }

        public async Task<DirectJobStatusResponse> GetStatus(DirectJobReference directJobReference)
        {
            var response = await HttpClient.GetAsync(directJobReference.Reference);
            var content = response.Content.ReadAsStringAsync().Result;

            return DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<DirectJobStatusResponseDataTransferObject>(content));
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
