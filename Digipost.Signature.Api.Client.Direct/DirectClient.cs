using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Internal;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using log4net;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectClient : BaseClient
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Uri _subPath;

        public DirectClient(ClientConfiguration clientConfiguration)
            : base(clientConfiguration)
        {
            _subPath = new Uri($"/api/{clientConfiguration.Sender.OrganizationNumber}/direct/signature-jobs", UriKind.Relative);
            Log.Debug($"Creating DirectClient, endpoint {new Uri(clientConfiguration.Environment.Url, _subPath)}");
        }

        public async Task<DirectJobResponse> Create(DirectJob directJob)
        {
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, directJob.Document, directJob.Signer, ClientConfiguration.Certificate);
            var createAction = new DirectCreateAction(directJob, documentBundle);
            var directJobResponse = await RequestHelper.Create(_subPath, createAction.Content(), DirectCreateAction.DeserializeFunc);

            Log.Debug($"Successfully created Direct Job with JobId: {directJobResponse.JobId}.");

            return directJobResponse;
        }

        public async Task<DirectJobStatusResponse> GetStatus(StatusReference statusReference)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = statusReference.Url(),
                Method = HttpMethod.Get
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var requestResult = await HttpClient.SendAsync(request);
            var requestContent = requestResult.Content.ReadAsStringAsync().Result;

            switch (requestResult.StatusCode)
            {
                case HttpStatusCode.OK:
                    var directJobStatusResponse = DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<directsignaturejobstatusresponse>(requestContent));
                    Log.Debug($"Requested status for JobId: {directJobStatusResponse.JobId}, status was: {directJobStatusResponse.Status}.");
                    return directJobStatusResponse;
                default:
                    throw RequestHelper.HandleGeneralException(requestContent, requestResult.StatusCode);
            }
        }

        public async Task<Stream> GetXades(XadesReference xadesReference)
        {
            return await RequestHelper.GetStream(xadesReference.Url);
        }

        public async Task<Stream> GetPades(PadesReference padesReference)
        {
            return await RequestHelper.GetStream(padesReference.Url);
        }

        public async Task Confirm(ConfirmationReference confirmationReference)
        {
            await RequestHelper.Confirm(confirmationReference);
        }

        internal async Task<string> AutoSign(long jobId)
        {
            Log.Warn($"Autosigning DirectJob with id: `{jobId}`. Should only happen in tests.");
            var url = new Uri($"/web/signature-jobs/{jobId}/devmodesign", UriKind.Relative);
            var httpResponseMessage = await HttpClient.PostAsync(url, null);
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}