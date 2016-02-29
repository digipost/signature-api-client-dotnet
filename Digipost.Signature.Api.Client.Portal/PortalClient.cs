using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;
using Digipost.Signature.Api.Client.Portal.Exceptions;
using Digipost.Signature.Api.Client.Portal.Internal;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalClient : BaseClient
    {
        private const int TooManyRequestsStatusCode = 429;
        private const string NextPermittedPollTimeHeader = "X-Next-permitted-poll-time";
        private readonly Uri _subPath;

        public PortalClient(ClientConfiguration clientConfiguration)
            : base(clientConfiguration)
        {
            _subPath = new Uri($"/api/{clientConfiguration.Sender.OrganizationNumber}/portal/signature-jobs", UriKind.Relative);
        }

        public async Task<PortalJobResponse> Create(PortalJob portalJob)
        {
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, portalJob.Document, portalJob.Signers, ClientConfiguration.Certificate);
            var portalCreateAction = new PortalCreateAction(portalJob, documentBundle);

            return await RequestHelper.Create(_subPath, portalCreateAction.Content(), PortalCreateAction.DeserializeFunc);
        }

        public async Task<PortalJobStatusChangeResponse> GetStatusChange()
        {
            PortalJobStatusChangeResponse portalJobStatusChangeResponse = null;

            var request = new HttpRequestMessage
            {
                RequestUri = _subPath,
                Method = HttpMethod.Get
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var requestResult = await HttpClient.SendAsync(request);
            var requestContent = await requestResult.Content.ReadAsStringAsync();

            switch (requestResult.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;
                case HttpStatusCode.OK:
                    portalJobStatusChangeResponse = await ParseResponseToPortalJobStatusChangeResponse(requestContent);
                    break;
                case (HttpStatusCode) TooManyRequestsStatusCode:
                    var nextPermittedPollTime = requestResult.Headers.GetValues(NextPermittedPollTimeHeader).FirstOrDefault();
                    throw new TooEagerPollingException(nextPermittedPollTime);
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
                    break;
                case HttpStatusCode.Conflict:
                    throw new JobCompletedException();
                default:
                    throw RequestHelper.HandleGeneralException(await requestResult.Content.ReadAsStringAsync(), requestResult.StatusCode);
            }
        }

        internal async Task AutoSign(int jobId, string signer)
        {
            var url = new Uri($"/web/portal/signature-jobs/{jobId}/devmodesign?signer={signer}", UriKind.Relative);
            await HttpClient.PostAsync(url, null);
        }
    }
}