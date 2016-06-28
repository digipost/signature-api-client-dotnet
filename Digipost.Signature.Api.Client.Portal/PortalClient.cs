using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Common.Logging;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;
using Digipost.Signature.Api.Client.Portal.Enums;
using Digipost.Signature.Api.Client.Portal.Exceptions;
using Digipost.Signature.Api.Client.Portal.Internal;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
using Digipost.Signature.Api.Client.Scripts.XsdToCode.Code;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalClient : BaseClient
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PortalClient(ClientConfiguration clientConfiguration)
            : base(clientConfiguration)
        {
        }

        public async Task<JobResponse> Create(Job job)
        {
            job.Sender = CurrentSender(job.Sender);
            var relativeUrl = RelativeUrl(job.Sender);

            var documentBundle = PortalAsiceGenerator.CreateAsice(job, ClientConfiguration.Certificate, ClientConfiguration);
            var portalCreateAction = new CreateAction(job, documentBundle);
            var portalJobResponse = await RequestHelper.Create(relativeUrl, portalCreateAction.Content(), CreateAction.DeserializeFunc);

            Log.Debug($"Successfully created Portal Job with JobId: {portalJobResponse.JobId}.");

            return portalJobResponse;
        }

        /// <summary>
        ///     If there is a job with an updated <see cref="JobStatus" />, the returned object contains necessary information to
        ///     act
        ///     on the status change. If the returned object has status <see cref="JobStatus.NoChanges" />, there is no changes.
        ///     When
        ///     processing of the status change is complete, (e.g. retrieving  <see cref="GetPades(PadesReference)">Pades</see>
        ///     and/or <see cref="GetXades(XadesReference)">Xades</see> documents for a <see cref="JobStatus.Completed" /> job
        ///     where
        ///     all signers have <see cref="SignatureStatus">signed</see> their documents), the returned status must be
        ///     <see cref="Confirm(ConfirmationReference)">confirmed</see>.
        /// </summary>
        /// <param name="sender">
        ///     The organization the status change is requested on behalf of. Defaults to
        ///     <see cref="ClientConfiguration.GlobalSender" />
        /// </param>
        /// <returns>the changed status for a job, never null.</returns>
        public async Task<JobStatusChanged> GetStatusChange(Sender sender = null)
        {
            JobStatusChanged jobStatusChanged;

            var request = new HttpRequestMessage
            {
                RequestUri = RelativeUrl(CurrentSender(sender)),
                Method = HttpMethod.Get
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var requestResult = await HttpClient.SendAsync(request);
            var requestContent = await requestResult.Content.ReadAsStringAsync();

            Log.Debug($"Requesting status change on endpoint {requestResult.RequestMessage.RequestUri} ...");

            switch (requestResult.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    Log.Debug("No content response received.");
                    jobStatusChanged = JobStatusChanged.NoChangesJobStatusChanged;
                    break;
                case HttpStatusCode.OK:
                    jobStatusChanged = ParseResponseToPortalJobStatusChangeResponse(requestContent);
                    Log.Debug($"JobStatusChangeResponse received: JobId: {jobStatusChanged.JobId}, JobStatus: {jobStatusChanged.Status}");
                    break;
                case (HttpStatusCode) TooManyRequestsStatusCode:
                    var nextPermittedPollTime = requestResult.Headers.GetValues(NextPermittedPollTimeHeader).FirstOrDefault();
                    var tooEagerPollingException = new TooEagerPollingException(nextPermittedPollTime);

                    Log.Warn(tooEagerPollingException.Message);

                    throw tooEagerPollingException;
                default:
                    throw RequestHelper.HandleGeneralException(requestContent, requestResult.StatusCode);
            }

            return jobStatusChanged;
        }

        private static Uri RelativeUrl(Sender sender)
        {
            return new Uri($"/api/{sender.OrganizationNumber}/portal/signature-jobs", UriKind.Relative);
        }

        private static JobStatusChanged ParseResponseToPortalJobStatusChangeResponse(string requestContent)
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

        /// <summary>
        ///     Confirms that the status retrieved from <see cref="GetStatusChange(Sender)">GetStatusChange</see> is received and
        ///     may be discarded by the
        ///     Signature service and not retrieved again. Calling this method on a status update without
        ///     <see cref="ConfirmationReference" /> has no effect.
        /// </summary>
        /// <param name="confirmationReference">
        ///     the updated status retrieved from
        ///     <see cref="GetStatusChange(Sender)">GetStatusChange</see>{@link #getStatusChange()}
        /// </param>
        /// <returns></returns>
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
                    Log.Debug($"PortalJob cancelled successfully [CancellationReference: {cancellationReference.Url}].");
                    break;
                case HttpStatusCode.Conflict:
                    Log.Debug($"PortalJob was not cancelled. Job was already completed [CancellationReference: {cancellationReference.Url}].");
                    throw new JobCompletedException();
                default:
                    throw RequestHelper.HandleGeneralException(await requestResult.Content.ReadAsStringAsync(), requestResult.StatusCode);
            }
        }

        internal async Task<HttpResponseMessage> AutoSign(int jobId, string signer)
        {
            Log.Warn($"Autosigning PortalJob with id: `{jobId}` for signer:`{signer}`. Should only happen in tests.");
            var url = new Uri($"/web/portal/signature-jobs/{jobId}/devmodesign?signer={signer}", UriKind.Relative);
            var httpResponseMessage = await HttpClient.PostAsync(url, null);
            return httpResponseMessage;
        }
    }
}