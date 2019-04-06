using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Internal.Enums;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;
using Digipost.Signature.Api.Client.Portal.Enums;
using Digipost.Signature.Api.Client.Portal.Exceptions;
using Digipost.Signature.Api.Client.Portal.Internal;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalClient : BaseClient
    {
        private readonly ILogger<PortalClient> _logger;

        public PortalClient(ClientConfiguration clientConfiguration)
            : this(clientConfiguration, new NullLoggerFactory())
        {
        }

        public PortalClient(ClientConfiguration clientConfiguration, ILoggerFactory loggerFactory)
            : base(clientConfiguration, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PortalClient>();
        }

        public async Task<JobResponse> Create(Job job)
        {
            job.Sender = CurrentSender(job.Sender);
            var relativeUrl = RelativeUrl(job.Sender, JobType.Portal, HttpMethod.Post);

            var documentBundle = PortalAsiceGenerator.CreateAsice(job, ClientConfiguration.Certificate, ClientConfiguration);
            var portalCreateAction = new CreateAction(job, documentBundle);
            var portalJobResponse = await RequestHelper.Create(relativeUrl, portalCreateAction.Content(), CreateAction.DeserializeFunc).ConfigureAwait(false);

            _logger.LogDebug($"Successfully created Portal Job with JobId: {portalJobResponse.JobId}.");

            return portalJobResponse;
        }

        /// <summary>
        ///     If there is a job with an updated <see cref="JobStatus" />, the returned object contains necessary information to
        ///     act
        ///     on the status change. If the returned object has status <see cref="JobStatus.NoChanges" />, there is no changes.
        ///     When
        ///     processing of the status change is complete, (e.g. retrieving  <see cref="GetPades(PadesReference)">Pades</see>
        ///     and/or <see cref="GetXades(XadesReference)">Xades</see> documents for a
        ///     <see cref="JobStatus.CompletedSuccessfully" /> job
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
            var request = new HttpRequestMessage
            {
                RequestUri = RelativeUrl(CurrentSender(sender), JobType.Portal, HttpMethod.Get),
                Method = HttpMethod.Get
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var requestResult = await HttpClient.SendAsync(request).ConfigureAwait(false);
            var requestContent = await requestResult.Content.ReadAsStringAsync().ConfigureAwait(false);

            _logger.LogDebug($"Requesting status change on endpoint {requestResult.RequestMessage.RequestUri} ...");

            switch (requestResult.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return CreateNoContentResponse(requestResult);
                case HttpStatusCode.OK:
                    return CreateOkResponse(requestContent, requestResult);
                case HttpStatusCode.TooManyRequests:
                    throw CreateTooManyRequestsException(requestResult);
                default:
                    throw RequestHelper.HandleGeneralException(requestContent, requestResult.StatusCode);
            }
        }

        private JobStatusChanged CreateNoContentResponse(HttpResponseMessage requestResult)
        {
            _logger.LogDebug("No content response received.");
            return JobStatusChanged.NoChanges(RequestHelper.GetNextPermittedPollTime(requestResult));
        }
        
        private JobStatusChanged CreateOkResponse(string requestContent, HttpResponseMessage requestResult)
        {
            var jobStatusChanged = ParseResponseToPortalJobStatusChangeResponse(requestContent, RequestHelper.GetNextPermittedPollTime(requestResult));
            _logger.LogDebug($"JobStatusChangeResponse received: JobId: {jobStatusChanged.JobId}, JobStatus: {jobStatusChanged.Status}");
            return jobStatusChanged;
        }

        
        private TooEagerPollingException CreateTooManyRequestsException(HttpResponseMessage requestResult)
        {
            var nextPermittedPollTime =
                RequestHelper.IsBlockedByDosFilter(requestResult, ClientConfiguration.DosFilterHeaderBlockKey)
                    ? DateTime.Now.Add(ClientConfiguration.DosFilterBlockingPeriod)
                    : RequestHelper.GetNextPermittedPollTime(requestResult);

            var tooEagerPollingException = new TooEagerPollingException(nextPermittedPollTime);
            _logger.LogWarning(tooEagerPollingException.Message);
            return tooEagerPollingException;
        }

        public async Task<string> GetRootResource(Sender sender)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"/api/{sender.OrganizationNumber}", UriKind.Relative),
                Method = HttpMethod.Get
            };
           
            try
            {
                var requestResult = await HttpClient.SendAsync(request).ConfigureAwait(false);
                var requestContent = await requestResult.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogDebug($"Requested status change on endpoint {requestResult.RequestMessage.RequestUri} ...");
                return requestContent;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
        
        private static JobStatusChanged ParseResponseToPortalJobStatusChangeResponse(string requestContent, DateTime nextPermittedPollTime)
        {
            var deserialized = SerializeUtility.Deserialize<portalsignaturejobstatuschangeresponse>(requestContent);
            var portalJobStatusChangeResponse = DataTransferObjectConverter.FromDataTransferObject(deserialized,nextPermittedPollTime );
            return portalJobStatusChangeResponse;
        }

        public async Task<Stream> GetXades(XadesReference xadesReference)
        {
            return await RequestHelper.GetStream(xadesReference.Url).ConfigureAwait(false);
        }

        public async Task<Stream> GetPades(PadesReference padesReference)
        {
            return await RequestHelper.GetStream(padesReference.Url).ConfigureAwait(false);
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
            await RequestHelper.Confirm(confirmationReference).ConfigureAwait(false);
        }

        public async Task Cancel(CancellationReference cancellationReference)
        {
            var requestResult = await HttpClient.PostAsync(cancellationReference.Url, null).ConfigureAwait(false);
            switch (requestResult.StatusCode)
            {
                case HttpStatusCode.OK:
                    _logger.LogDebug($"PortalJob cancelled successfully [CancellationReference: {cancellationReference.Url}].");
                    break;
                case HttpStatusCode.Conflict:
                    _logger.LogDebug($"PortalJob was not cancelled. Job was already completed [CancellationReference: {cancellationReference.Url}].");
                    throw new JobCompletedException();
                default:
                    throw RequestHelper.HandleGeneralException(await requestResult.Content.ReadAsStringAsync().ConfigureAwait(false), requestResult.StatusCode);
            }
        }

        internal async Task<HttpResponseMessage> AutoSign(int jobId, string signer)
        {
            _logger.LogWarning($"Autosigning PortalJob with id: `{jobId}` for signer:`{signer}`. Should only happen in tests.");
            var url = new Uri($"/web/portal/signature-jobs/{jobId}/devmodesign?signer={signer}", UriKind.Relative);
            var httpResponseMessage = await HttpClient.PostAsync(url, null).ConfigureAwait(false);
            return httpResponseMessage;
        }
    }
}
