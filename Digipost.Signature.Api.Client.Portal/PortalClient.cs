using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;
using Digipost.Signature.Api.Client.Portal.Exceptions;
using Digipost.Signature.Api.Client.Portal.Internal;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
using log4net;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalClient : BaseClient
    {
        private const int TooManyRequestsStatusCode = 429;
        private const string NextPermittedPollTimeHeader = "X-Next-permitted-poll-time";

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Uri _subPath;

        public PortalClient(ClientConfiguration clientConfiguration)
            : base(clientConfiguration)
        {
            _subPath = new Uri($"/api/{clientConfiguration.Sender.OrganizationNumber}/portal/signature-jobs", UriKind.Relative);

            Log.Info($"Creating PortalClient, endpoint `{new Uri(clientConfiguration.Environment.Url, _subPath)}`");
        }

        public async Task<PortalJobResponse> Create(PortalJob portalJob)
        {
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, portalJob.Document, portalJob.Signers, ClientConfiguration.Certificate);
            var portalCreateAction = new PortalCreateAction(portalJob, documentBundle);
            var portalJobResponse = await RequestHelper.Create(_subPath, portalCreateAction.Content(), PortalCreateAction.DeserializeFunc);

            Log.Info($"Successfully created Portal Job with JobId: {portalJobResponse.JobId}.");

            return portalJobResponse;
        }

        public async Task<PortalJobStatusChangeResponse> GetStatusChange()
        {
            PortalJobStatusChangeResponse portalJobStatusChangeResponse;

            var request = new HttpRequestMessage
            {
                RequestUri = _subPath,
                Method = HttpMethod.Get
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var requestResult = await HttpClient.SendAsync(request);
            var requestContent = await requestResult.Content.ReadAsStringAsync();

            Log.Info($"Requesting status change on endpoint {requestResult.RequestMessage.RequestUri} ...");

            switch (requestResult.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    Log.Info("No content response received.");
                    portalJobStatusChangeResponse = PortalJobStatusChangeResponse.NoChangesJobStatusChangeResponse;
                    break;
                case HttpStatusCode.OK:
                    portalJobStatusChangeResponse = await ParseResponseToPortalJobStatusChangeResponse(requestContent);
                    Log.Info($"JobStatusChangeResponse received: JobId: {portalJobStatusChangeResponse.JobId}, JobStatus: {portalJobStatusChangeResponse.Status}");
                    break;
                case (HttpStatusCode) TooManyRequestsStatusCode:
                    var nextPermittedPollTime = requestResult.Headers.GetValues(NextPermittedPollTimeHeader).FirstOrDefault();
                    var tooEagerPollingException = new TooEagerPollingException(nextPermittedPollTime);
                    Log.Warn(tooEagerPollingException.Message);
                    throw tooEagerPollingException;
                default:
                    throw RequestHelper.HandleGeneralException(requestContent, requestResult.StatusCode);
            }

            return portalJobStatusChangeResponse;
        }

        private static async Task<PortalJobStatusChangeResponse> ParseResponseToPortalJobStatusChangeResponse(string requestContent)
        {
            var deserialized = SerializeUtility.Deserialize<portalsignaturejobstatuschangeresponse>(requestContent);
            var portalJobStatusChangeResponse = DataTransferObjectConverter.FromDataTransferObject(deserialized);
            return portalJobStatusChangeResponse;
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

        public async Task Cancel(CancellationReference cancellationReference)
        {
            var requestResult = await HttpClient.PostAsync(cancellationReference.Url, null);
            switch (requestResult.StatusCode)
            {
                case HttpStatusCode.OK:
                    Log.Info($"PortalJob cancelled successfully [CancellationReference: {cancellationReference.Url}].");
                    break;
                case HttpStatusCode.Conflict:
                    Log.Info($"PortalJob was not cancelled. Job was already completed [CancellationReference: {cancellationReference.Url}].");
                    throw new JobCompletedException();
                default:
                    throw RequestHelper.HandleGeneralException(await requestResult.Content.ReadAsStringAsync(), requestResult.StatusCode);
            }
        }

        internal async Task AutoSign(int jobId, string signer)
        {
            Log.Warn($"Autosigning PortalJob with id: `{jobId}` for signer:`{signer}`. Should only happen in tests.");
            var url = new Uri($"/web/portal/signature-jobs/{jobId}/devmodesign?signer={signer}", UriKind.Relative);
            await HttpClient.PostAsync(url, null);
        }
    }
}