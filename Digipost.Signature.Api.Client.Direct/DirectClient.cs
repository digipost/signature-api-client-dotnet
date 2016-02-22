using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;
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
            _subPath = new Uri($"/api/{clientConfiguration.Sender.OrganizationNumber}/direct/signature-jobs", UriKind.Relative);
        }

        public async Task<DirectJobResponse> Create(DirectJob directJob)
        {
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, directJob.Document, directJob.Signer, ClientConfiguration.Certificate);
            var createAction = new DirectCreateAction(directJob, documentBundle);

            var request = new HttpRequestMessage
            {
                RequestUri = _subPath,
                Method = HttpMethod.Post,
                Content = createAction.Content()
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var requestResult = await HttpClient.SendAsync(request);
            var requestContent = await requestResult.Content.ReadAsStringAsync();

            return DirectCreateAction.DeserializeFunc(requestContent);
        }

        public async Task<DirectJobStatusResponse> GetStatus(StatusReference statusReference)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = statusReference.Url,
                Method = HttpMethod.Get,
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var requestResult = await HttpClient.SendAsync(request);
            var requestContent = requestResult.Content.ReadAsStringAsync().Result;

            return DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<directsignaturejobstatusresponse>(requestContent));
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
            return await HttpClient.PostAsync(confirmationReference.Url, content: null);
        }

        internal async Task AutoSign(long jobId)
        {
            var url = new Uri($"/web/signature-jobs/{jobId}/devmodesign", UriKind.Relative);
            await HttpClient.PostAsync(url, content: null);
        }
    }
}
