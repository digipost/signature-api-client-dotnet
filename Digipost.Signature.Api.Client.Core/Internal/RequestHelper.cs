﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Internal.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Schemas;

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
                throw HandleGeneralException(responseMessage.StatusCode, responseContent);
            }

            return deserializeFunc(responseContent);
        }

        public async Task<directsignerresponse> RequestNewRedirectUrl(Uri signerUrl)
        {
            var responseMessage = await _httpClient.PostAsync(signerUrl, null).ConfigureAwait(false);
            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            return SerializeUtility.Deserialize<directsignerresponse>(responseContent);
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
                var content = await requestResult.Content.ReadAsStringAsync().ConfigureAwait(false);
                
                throw HandleGeneralException(requestResult.StatusCode, content);
            }

            return await requestResult.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        public async Task Confirm(ConfirmationReference confirmationReference)
        {
            var requestResult = await _httpClient.PostAsync(confirmationReference.Url, null).ConfigureAwait(false);
            var requestResultContent = await requestResult.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            if (!requestResult.IsSuccessStatusCode)
            {
                throw HandleGeneralException(requestResult.StatusCode, requestResultContent);
            }

            _logger.LogDebug($"Successfully confirmed job with confirmation reference: {confirmationReference.Url}");
        }

        internal SignatureException HandleGeneralException(HttpStatusCode statusCode, string requestContent = null)
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

            if (error != null && error.Code == BrokerNotAuthorized)
            {
                return new BrokerNotAuthorizedException(error, statusCode);
            }

            _logger.LogWarning($"Unexpected error occured. Content: `{requestContent}`, statusCode: {statusCode}");
            return new UnexpectedResponseException(error, statusCode);
        }

        internal bool IsBlockedByDosFilter(HttpResponseMessage requestResult, string dosFilterHeaderBlockKey)
        {
            return requestResult.Headers.TryGetValues(dosFilterHeaderBlockKey, out _);
        }

        internal DateTime GetNextPermittedPollTime(HttpResponseMessage requestResult)
        {
            return DateTime.Parse(requestResult.Headers.GetValues(BaseClient.NextPermittedPollTimeHeader).FirstOrDefault());
        }
    }
}
