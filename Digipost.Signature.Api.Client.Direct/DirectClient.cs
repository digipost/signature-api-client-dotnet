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
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Internal;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectClient : BaseClient
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DirectClient(ClientConfiguration clientConfiguration)
            : base(clientConfiguration)
        {
        }

        public async Task<JobResponse> Create(Job job)
        {
            job.Sender = CurrentSender(job.Sender);
            var relativeUrl = RelativeUrl(job.Sender);

            var documentBundle = DirectAsiceGenerator.CreateAsice(job, ClientConfiguration.Certificate, ClientConfiguration);
            var createAction = new CreateAction(job, documentBundle);
            var directJobResponse = await RequestHelper.Create(relativeUrl, createAction.Content(), CreateAction.DeserializeFunc);

            Log.Debug($"Successfully created Direct Job with JobId: {directJobResponse.JobId}.");

            return directJobResponse;
        }

        private static Uri RelativeUrl(Sender sender)
        {
            return new Uri($"/api/{sender.OrganizationNumber}/direct/signature-jobs", UriKind.Relative);
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

            var requestResult = await HttpClient.SendAsync(request);
            var requestContent = requestResult.Content.ReadAsStringAsync().Result;

            switch (requestResult.StatusCode)
            {
                case HttpStatusCode.OK:
                    var jobStatusResponse = DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<directsignaturejobstatusresponse>(requestContent));
                    Log.Debug($"Requested status for JobId: {jobStatusResponse.JobId}, status was: {jobStatusResponse.Status}.");
                    return jobStatusResponse;
                default:
                    throw RequestHelper.HandleGeneralException(requestContent, requestResult.StatusCode);
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
                    Log.Debug("Received empty response. No jobs have had their status changed.");
                    return JobStatusResponse.NoChanges;
                case HttpStatusCode.OK:
                    var changedJob = ParseResponseToJobStatusResponse(requestContent);
                    Log.Debug($"Received updated status. Job with id {changedJob.JobId} has status {changedJob.Status}.");
                    return changedJob;
                case (HttpStatusCode) TooManyRequestsStatusCode:
                    var nextPermittedPollTime = requestResult.Headers.GetValues(NextPermittedPollTimeHeader).FirstOrDefault();
                    var tooEagerPollingException = new TooEagerPollingException(nextPermittedPollTime);

                    Log.Warn(tooEagerPollingException.Message);

                    throw tooEagerPollingException;
                default:
                    throw RequestHelper.HandleGeneralException(requestContent, requestResult.StatusCode);
            }
        }

        private static JobStatusResponse ParseResponseToJobStatusResponse(string requestContent)
        {
            var deserialized = SerializeUtility.Deserialize<directsignaturejobstatusresponse>(requestContent);
            return DataTransferObjectConverter.FromDataTransferObject(deserialized);
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
        ///     Confirms that the status retrieved from <see cref="GetStatus(StatusReference)" /> is received. If the confirmed
        ///     <see cref="JobStatus" /> is a terminal status (i.e. <see cref="JobStatus.CompletedSuccessfully" /> or
        ///     <see cref="JobStatus.Failed" />),
        ///     the Signature service may make the job's associated resources unavailable through the API when receiving the
        ///     confirmation. Calling this method for a response with no <see cref="ConfirmationReference" /> has no effect.
        /// </summary>
        /// <param name="confirmationReference">the updated status retrieved from <see cref="GetStatus(StatusReference)" /></param>
        public async Task Confirm(ConfirmationReference confirmationReference)
        {
            await RequestHelper.Confirm(confirmationReference);
        }

        internal async Task<string> AutoSign(long jobId, string signer)
        {
            Log.Warn($"Autosigning DirectJob with id: `{jobId}` for signer:`{signer}`. Should only happen in tests.");
            var url = new Uri($"/web/signature-jobs/{jobId}/devmodesign?signer={signer}", UriKind.Relative);
            var httpResponseMessage = await HttpClient.PostAsync(url, null);
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}