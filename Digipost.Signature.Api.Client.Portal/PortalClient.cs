using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Portal.Internal;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
using DataTransferObjectConverter = Digipost.Signature.Api.Client.Portal.DataTransferObjects.DataTransferObjectConverter;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalClient : BaseClient
    {
        private readonly Uri _subPath;
        private const int TooManyRequestsStatusCode = 429;
        private const string  NextPermittedPollTimeHeader = "X-Next-permitted-poll-time";
        private const string BrokerNotAuthorized = "BROKER_NOT_AUTHORIZED";

        public PortalClient(ClientConfiguration clientConfiguration) 
            : base(clientConfiguration)
        {
            _subPath = new Uri(string.Format("/api/{0}/portal/signature-jobs", clientConfiguration.Sender.OrganizationNumber), UriKind.Relative);
        }

        public async Task<PortalJobResponse> Create(PortalJob portalJob)
        {
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, portalJob.Document,portalJob.Signers, ClientConfiguration.Certificate);
            var portalCreateAction = new PortalCreateAction(portalJob, documentBundle);

            var request = new HttpRequestMessage
            {
                RequestUri = _subPath,
                Method = HttpMethod.Post,
                Content = portalCreateAction.Content()
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            var requestResult = await HttpClient.PostAsync(_subPath, portalCreateAction.Content());
            var requestContent = await requestResult.Content.ReadAsStringAsync();

            return PortalCreateAction.DeserializeFunc(requestContent);
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
                    throw HandleGeneralException(requestContent, requestResult.StatusCode);
            }

            return portalJobStatusChangeResponse;
        }

        private SignatureException HandleGeneralException(string requestContent, HttpStatusCode statusCode)
        {
            Error error;
            try
            {
                error = Core.DataTransferObjects.DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<error>(requestContent));
            }
            catch (Exception e)
            {
                return new UnexpectedResponseException(requestContent, statusCode, e);
            }
            
            if (error.Code == BrokerNotAuthorized)
            {
                return new BrokerNotAuthorizedException(error, statusCode);   
            } 

            return new UnexpectedResponseException(error, statusCode);
        }

        private static async Task<PortalJobStatusChangeResponse> ParseResponseToPortalJobStatusChangeResponse(string requestContent)
        {
            var deserialized = SerializeUtility.Deserialize<portalsignaturejobstatuschangeresponse>(requestContent);
            var portalJobStatusChangeResponse = DataTransferObjectConverter.FromDataTransferObject(deserialized);
            return portalJobStatusChangeResponse;
        }

        public async Task<Stream> GetXades(XadesReference xadesReference)
        {
            return await HttpClient.GetStreamAsync(xadesReference.Url);
        }

        public async Task<Stream> GetPades(PadesReference padesReference)
        {
            return await HttpClient.GetStreamAsync(padesReference.Url);
        }

        public async Task Confirm(ConfirmationReference confirmationReference)
        {
            await HttpClient.PostAsync(confirmationReference.Url, content: null);
        }

        internal async Task AutoSign(int jobId, string signer)
        {
            var url = new Uri(string.Format("/web/portal/signature-jobs/{0}/devmodesign?signer={1}", jobId, signer), UriKind.Relative);
            await HttpClient.PostAsync(url, content: null);
        }

    }
}
