using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Common.Logging;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
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

        public async Task<DirectJobResponse> Create(DirectJob directJob)
        {
            directJob.Sender = CurrentSender(directJob.Sender);
            var relativeUrl = RelativeUrl(directJob);

            var documentBundle = DirectAsiceGenerator.CreateAsice(directJob, ClientConfiguration.Certificate, ClientConfiguration);
            var createAction = new DirectCreateAction(directJob, documentBundle);
            var directJobResponse = await RequestHelper.Create(relativeUrl, createAction.Content(), DirectCreateAction.DeserializeFunc);

            Log.Debug($"Successfully created Direct Job with JobId: {directJobResponse.JobId}.");

            return directJobResponse;
        }

        private static Uri RelativeUrl(DirectJob directJob)
        {
            return new Uri($"/api/{directJob.Sender.OrganizationNumber}/direct/signature-jobs", UriKind.Relative);
        }

        /// <summary>
        ///     Get the current status for the given <see cref="StatusReference" />, which references the status for a specific
        ///     job.
        ///     When processing of the status is complete (e.g. retrieving <see cref="GetPades(PadesReference)" /> and/or
        ///     <see cref="GetXades(XadesReference)" /> documents for a <see cref="JobStatus.Signed" /> job), the returned  status
        ///     must be confirmed via <see cref="Confirm(ConfirmationReference)" />.
        /// </summary>
        /// <param name="statusReference">The reference to the status of a specific job.</param>
        /// <returns>
        ///     the <see cref="DirectJobStatusResponse" /> for the job referenced by the given <see cref="StatusReference" />,
        ///     never null.
        /// </returns>
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

        /// <summary>
        ///     Confirms that the status retrieved from <see cref="GetStatus(StatusReference)" /> is received. If the confirmed
        ///     <see cref="JobStatus" /> is a terminal status (e.g. <see cref="JobStatus.Signed" /> or
        ///     <see cref="JobStatus.Rejected" />),
        ///     the Signature service may make the job's associated resources unavailable through the API when receiving the
        ///     confirmation. Calling this method for a response with no <see cref="ConfirmationReference" /> has no effect.
        /// </summary>
        /// <param name="confirmationReference">the updated status retrieved from <see cref="GetStatus(StatusReference)" /></param>
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