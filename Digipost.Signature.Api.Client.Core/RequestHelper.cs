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
        private readonly HttpClient _httpClient;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


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
            
            Log.Info("Create request sent");

            if (!responseMessage.IsSuccessStatusCode)
            {
                Log.Error($"Create request sent, but failed {responseMessage.StatusCode}, {responseMessage.ReasonPhrase})");
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

            var requestResult = await _httpClient.SendAsync(request);

            if (!requestResult.IsSuccessStatusCode)
            {
                throw HandleGeneralException(await requestResult.Content.ReadAsStringAsync(), requestResult.StatusCode);
            }

            return await requestResult.Content.ReadAsStreamAsync();
        }

        public async Task Confirm(ConfirmationReference confirmationReference)
        {
            var requestResult = await _httpClient.PostAsync(confirmationReference.Url, null);

            if (!requestResult.IsSuccessStatusCode)
            {
                throw HandleGeneralException(await requestResult.Content.ReadAsStringAsync(), requestResult.StatusCode);
            }
        }

        internal SignatureException HandleGeneralException(string requestContent, HttpStatusCode statusCode)
        {
            Error error;
            try
            {
                error = DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<error>(requestContent));
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
    }
}