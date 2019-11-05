using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Internal.Enums;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Internal;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Digipost.Signature.Api.Client.Direct.NewRedirectUrl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Schemas;

// ReSharper disable MemberCanBePrivate.Global

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectClient : BaseClient
    {
        private readonly ILogger<BaseClient> _logger;

        public DirectClient(ClientConfiguration clientConfiguration)
            : this(clientConfiguration, new NullLoggerFactory())
        {
        }

        public DirectClient(ClientConfiguration clientConfiguration, ILoggerFactory loggerFactory)
            : base(clientConfiguration, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DirectClient>();
        }

        public async Task<JobResponse> Create(Job job)
        {
            job.Sender = CurrentSender(job.Sender);
            var relativeUrl = RelativeUrl(job.Sender, JobType.Direct, HttpMethod.Post);

            var documentBundle = DirectAsiceGenerator.CreateAsice(job, ClientConfiguration.Certificate, ClientConfiguration);
            var createAction = new CreateAction(job, documentBundle);
            var directJobResponse = await RequestHelper.Create(relativeUrl, createAction.Content(), CreateAction.DeserializeFunc).ConfigureAwait(false);

            _logger.LogDebug($"Successfully created Direct Job with JobId: {directJobResponse.JobId}.");

            return directJobResponse;
        }

        public async Task<SignerResponse> RequestNewRedirectUrl(IWithSignerUrl signerUrl)
        {
            var newRedirectUrlResult = await RequestHelper.RequestNewRedirectUrl(signerUrl.SignerUrl);
            return new SignerResponse(newRedirectUrlResult);
        }

        /// <summary>
        ///     Get the current status for the given <see cref="StatusReference" />, which references the status for a specific
        ///     job.
        ///     When processing of the status is complete (e.g. retrieving <see cref="GetPades(PadesReference)">PAdES</see> and/or
        ///     <see cref="GetXades(XadesReference)">XAdES</see> documents for a <see cref="JobStatus.CompletedSuccessfully" /> job
        ///     where all the signers have <see cref="SignatureStatus.Signed" /> their documents), the returned  status
        ///     must be confirmed via <see cref="Confirm(ConfirmationReference)" />.
        /// </summary>
        /// <param name="statusReference">The reference to the status of a specific job.</param>
        /// <returns>
        ///     the <see cref="JobStatusResponse" /> for the job referenced by the given <see cref="StatusReference" />,
        ///     never null.
        /// </returns>
        public async Task<JobStatusResponse> GetStatus(StatusReference statusReference)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = statusReference.Url(),
                Method = HttpMethod.Get
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var requestResult = await HttpClient.SendAsync(request).ConfigureAwait(false);
            var requestContent = requestResult.Content.ReadAsStringAsync().Result;

            switch (requestResult.StatusCode)
            {
                case HttpStatusCode.OK:
                    var nextPermittedPollTime = DateTime.Now;
                    var jobStatusResponse = DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<directsignaturejobstatusresponse>(requestContent), nextPermittedPollTime);
                    _logger.LogDebug($"Requested status for JobId: {jobStatusResponse.JobId}, status was: {jobStatusResponse.Status}.");
                    return jobStatusResponse;
                default:
                    throw RequestHelper.HandleGeneralException(requestResult.StatusCode, requestContent);
            }
        }

        /// <summary>
        ///     If there is a job with an updated <see cref="JobStatus" />, the returned object contains necessary information to
        ///     act on the status change. If the returned object has status <see cref="JobStatus.NoChanges" />, there is no
        ///     changes.
        ///     When processing of the status change is complete, (e.g. retrieving <see cref="GetPades(PadesReference)">PAdES</see>
        ///     and/or <see cref="GetXades(XadesReference)">XAdES</see> documents for a
        ///     <see cref="JobStatus.CompletedSuccessfully" /> job
        ///     where all the signers have <see cref="SignatureStatus.Signed" /> their documents),
        ///     the returned status must be <see cref="Confirm(ConfirmationReference)">confirmed</see>.
        /// </summary>
        /// <param name="sender">
        ///     The organization the status change is requested on behalf of. Defaults to
        ///     <see cref="ClientConfiguration.GlobalSender" />
        /// </param>
        /// <returns>the changed status for a job, never null.</returns>
        public async Task<JobStatusResponse> GetStatusChange(Sender sender = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = RelativeUrl(CurrentSender(sender), JobType.Direct, HttpMethod.Get),
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
                    throw RequestHelper.HandleGeneralException(requestResult.StatusCode, requestContent);
            }
        }

        private TooEagerPollingException CreateTooManyRequestsException(HttpResponseMessage requestResult)
        {
            var nextPermittedPollTime =
                RequestHelper.IsBlockedByDosFilter(requestResult, ClientConfiguration.DosFilterHeaderBlockingKey)
                    ? DateTime.Now.Add(ClientConfiguration.DosFilterBlockingPeriod)
                    : RequestHelper.GetNextPermittedPollTime(requestResult);

            var tooEagerPollingException = new TooEagerPollingException(nextPermittedPollTime);
            _logger.LogWarning(tooEagerPollingException.Message);
            return tooEagerPollingException;
        }

        private JobStatusResponse CreateOkResponse(string requestContent, HttpResponseMessage requestResult)
        {
            var changedJob = ParseResponseToJobStatusResponse(requestContent, RequestHelper.GetNextPermittedPollTime(requestResult));
            _logger.LogDebug($"Received updated status. Job with id {changedJob.JobId} has status {changedJob.Status}.");
            return changedJob;
        }

        private JobStatusResponse CreateNoContentResponse(HttpResponseMessage requestResult)
        {
            _logger.LogDebug("Received empty response. No jobs have had their status changed.");
            return JobStatusResponse.NoChangesWithPollTime(RequestHelper.GetNextPermittedPollTime(requestResult));
        }

        private static JobStatusResponse ParseResponseToJobStatusResponse(string requestContent, DateTime nextPermittedPollTime)
        {
            var deserialized = SerializeUtility.Deserialize<directsignaturejobstatusresponse>(requestContent);
            return DataTransferObjectConverter.FromDataTransferObject(deserialized, nextPermittedPollTime);
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
        ///     Confirms that the status retrieved from <see cref="GetStatus(StatusReference)" /> is received. If the confirmed
        ///     <see cref="JobStatus" /> is a terminal status (i.e. <see cref="JobStatus.CompletedSuccessfully" /> or
        ///     <see cref="JobStatus.Failed" />),
        ///     the Signature service may make the job's associated resources unavailable through the API when receiving the
        ///     confirmation. Calling this method for a response with no <see cref="ConfirmationReference" /> has no effect.
        /// </summary>
        /// <param name="confirmationReference">the updated status retrieved from <see cref="GetStatus(StatusReference)" /></param>
        public async Task Confirm(ConfirmationReference confirmationReference)
        {
            await RequestHelper.Confirm(confirmationReference).ConfigureAwait(false);
        }

        internal async Task<string> AutoSign(long jobId, string signer)
        {
            _logger.LogWarning($"Autosigning DirectJob with id: `{jobId}` for signer:`{signer}`. Should only happen in tests.");
            var url = new Uri($"/web/signature-jobs/{jobId}/devmodesign?signer={signer}", UriKind.Relative);
            var httpResponseMessage = await HttpClient.PostAsync(url, null).ConfigureAwait(false);
            return await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}
