using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.DataTransferObjects;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;
using log4net;

namespace Digipost.Signature.Api.Client.Core
{
    internal class RequestHelper
    {
        private const string BrokerNotAuthorized = "BROKER_NOT_AUTHORIZED";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly HttpClient _httpClient;

        public RequestHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> Create<T>(Uri uri, HttpContent content, Func<string, T> deserializeFunc)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = HttpMethod.Post,
                Content = content
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var responseMessage = await _httpClient.SendAsync(request);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                Log.Error($"Create request sent, but failed: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase})");
                throw HandleGeneralException(responseContent, responseMessage.StatusCode);
            }

            var func = deserializeFunc(responseContent);
            return func;
        }

        public async Task<Stream> GetStream(Uri uri)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = HttpMethod.Get
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Octet));

            var requestResult = await _httpClient.SendAsync(request);

            Log.Info($"A stream was requested from {uri}");

            if (!requestResult.IsSuccessStatusCode)
            {
                Log.Error($"Unable to confirm job with confirmation reference: {uri}");
                throw HandleGeneralException(await requestResult.Content.ReadAsStringAsync(), requestResult.StatusCode);
            }

            return await requestResult.Content.ReadAsStreamAsync();
        }

        public async Task Confirm(ConfirmationReference confirmationReference)
        {
            var requestResult = await _httpClient.PostAsync(confirmationReference.Url, null);

            if (!requestResult.IsSuccessStatusCode)
            {
                Log.Error($"Unable to confirm job with confirmation reference: {confirmationReference.Url}");
                throw HandleGeneralException(await requestResult.Content.ReadAsStringAsync(), requestResult.StatusCode);
            }

            Log.Info($"Successfully confirmed job with confirmation reference: {confirmationReference.Url}");
        }

        internal SignatureException HandleGeneralException(string requestContent, HttpStatusCode statusCode)
        {
            Error error;
            try
            {
                error = DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<error>(requestContent));
                Log.Error($"Error occured: {error}");
            }
            catch (Exception exception)
            {
                Log.Error($"Unexpected error occured. Content: `{requestContent}`, statusCode: {statusCode}, {exception}");
                return new UnexpectedResponseException(requestContent, statusCode, exception);
            }

            if (error.Code == BrokerNotAuthorized)
            {
                Log.Error("Broker not authorized!");
                return new BrokerNotAuthorizedException(error, statusCode);
            }

            Log.Error($"Unexpected error occured. Content: `{requestContent}`, statusCode: {statusCode}");
            return new UnexpectedResponseException(error, statusCode);
        }
    }
}