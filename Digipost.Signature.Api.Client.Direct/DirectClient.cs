using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Internal;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectClient : BaseClient
    {
        private readonly Uri _subPath;

        public DirectClient(ClientConfiguration clientConfiguration)
            :base(clientConfiguration)
        {
            _subPath = new Uri(string.Format("/api/{0}/direct/signature-jobs", clientConfiguration.Sender.OrganizationNumber), UriKind.Relative);
        }

        public async Task<DirectJobResponse> Create(DirectJob directJob)
        {
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, directJob.Document, directJob.Signer, ClientConfiguration.Certificate);
            var createAction = new DirectCreateAction(directJob, documentBundle);
            var requestResult = await HttpClient.PostAsync(_subPath, createAction.Content());
            
            return DirectCreateAction.DeserializeFunc(await requestResult.Content.ReadAsStringAsync());
        }

        public async Task<DirectJobStatusResponse> GetStatus(StatusReference statusReference)
        {
            var response = await HttpClient.GetAsync(statusReference.Url);
            var content = response.Content.ReadAsStringAsync().Result;

            return DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<directsignaturejobstatusresponse>(content));
        }

        public async Task<Stream> GetXades(XadesReference xadesReference)
        {
            return await HttpClient.GetStreamAsync(xadesReference.Url);
        }

        public async Task<Stream> GetPades(PadesReference padesReference)
        {
            return await HttpClient.GetStreamAsync(padesReference.Url);
        }

        public async Task<HttpResponseMessage> Confirm(ConfirmationReference confirmationReference)
        {
            return await HttpClient.PostAsync(confirmationReference.ConfirmationUri, content: null);
        }
    }
}
