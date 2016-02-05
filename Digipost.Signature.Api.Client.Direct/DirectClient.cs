using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
            var createAction = new CreateAction(ClientConfiguration.Sender, directJob, documentBundle);
            var requestResult = await HttpClient.PostAsync(DirectJobSubPath, createAction.Content());
            
            return CreateAction.DeserializeFunc(await requestResult.Content.ReadAsStringAsync());
        }

        public async Task<DirectJobStatusResponse> GetStatus(StatusReference statusReference)
        {
            var response = await HttpClient.GetAsync(statusReference.Reference);
            var content = response.Content.ReadAsStringAsync().Result;

            return DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<DirectJobStatusResponseDataTransferObject>(content));
        }

        public async Task<Stream> GetXades(XadesReference xadesReference)
        {
            return await HttpClient.GetStreamAsync(xadesReference.XadesUri);
        }

        public async Task<Stream> GetPades(PadesReference padesReference)
        {
            return await HttpClient.GetStreamAsync(padesReference.PadesUri);
        }

        public async Task<HttpResponseMessage> Confirm(ConfirmationReference confirmationReference)
        {
            return await HttpClient.PostAsync(confirmationReference.ConfirmationUri, content: null);
        }
    }
}
