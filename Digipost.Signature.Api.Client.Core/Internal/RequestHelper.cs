using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Internal.DataTransferObjects;
using Microsoft.Extensions.Logging;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal class RequestHelper
    {
        private const string BrokerNotAuthorized = "BROKER_NOT_AUTHORIZED";
        private readonly HttpClient _httpClient;
        private ILogger<RequestHelper> _logger;

        public RequestHelper(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _logger = loggerFactory.CreateLogger<RequestHelper>();
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

            var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!responseMessage.IsSuccessStatusCode)
            {
                _logger.LogError($"Create request sent, but failed: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase})");
                throw HandleGeneralException(responseContent, responseMessage.StatusCode);
            }

            return deserializeFunc(responseContent);
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

            var requestResult = await _httpClient.SendAsync(request).ConfigureAwait(false);

            _logger.LogDebug($"A stream was requested from {uri}");

            if (!requestResult.IsSuccessStatusCode)
            {
                throw HandleGeneralException(await requestResult.Content.ReadAsStringAsync().ConfigureAwait(false), requestResult.StatusCode);
            }

            return await requestResult.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        public async Task Confirm(ConfirmationReference confirmationReference)
        {
            var requestResult = await _httpClient.PostAsync(confirmationReference.Url, null).ConfigureAwait(false);

            if (!requestResult.IsSuccessStatusCode)
            {
                throw HandleGeneralException(await requestResult.Content.ReadAsStringAsync().ConfigureAwait(false), requestResult.StatusCode);
            }

            _logger.LogDebug($"Successfully confirmed job with confirmation reference: {confirmationReference.Url}");
        }

        internal SignatureException HandleGeneralException(string requestContent, HttpStatusCode statusCode)
        {
            Error error;
            try
            {
                error = DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<error>(requestContent));
                _logger.LogWarning($"Error occured: {error}");
            }
            catch (Exception exception)
            {
                _logger.LogWarning($"Unexpected error occured. Content: `{requestContent}`, statusCode: {statusCode}, {exception}");
                return new UnexpectedResponseException(requestContent, statusCode, exception);
            }

            if (error.Code == BrokerNotAuthorized)
            {
                return new BrokerNotAuthorizedException(error, statusCode);
            }

            _logger.LogWarning($"Unexpected error occured. Content: `{requestContent}`, statusCode: {statusCode}");
            return new UnexpectedResponseException(error, statusCode);
        }
    }
}
