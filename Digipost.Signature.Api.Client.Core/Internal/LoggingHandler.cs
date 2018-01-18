﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal class LoggingHandler : DelegatingHandler
    {
//        private static readonly ILog Log = LogManager.GetLogger("Digipost.Signature.Api.Client.RequestResponse");

        public LoggingHandler(ClientConfiguration clientClientConfiguration)
        {
            ClientConfiguration = clientClientConfiguration;
        }

        public ClientConfiguration ClientConfiguration { get; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
//            if (ClientConfiguration.LogRequestAndResponse && Log.IsDebugEnabled)
//            {
//                await LogRequest(request).ConfigureAwait(false);
//            }

            var httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

//            if (ClientConfiguration.LogRequestAndResponse && Log.IsDebugEnabled)
//            {
//                await LogResponse(httpResponseMessage).ConfigureAwait(false);
//            }

            return httpResponseMessage;
        }

        private static async Task LogRequest(HttpRequestMessage request)
        {
            var requestData = await GetRequestData(request).ConfigureAwait(false);
//            Log.Debug($"Outgoing: {requestData}");
        }

        private static async Task LogResponse(HttpResponseMessage response)
        {
            var responseData = await GetResponseData(response).ConfigureAwait(false);
//            Log.Debug($"Incoming: {responseData}");
        }

        private static async Task<string> GetRequestData(HttpRequestMessage request)
        {
            var requestDescriptionsAndData = new List<KeyValuePair<string, string>>();
            requestDescriptionsAndData.Add(GetRequestMethodDescription(request));
            requestDescriptionsAndData.AddRange(GetHeadersDescription(request.Headers));
            requestDescriptionsAndData.AddRange(await GetRequestContentHeadersDescriptionAndData(request));

            return FormatHttpData(requestDescriptionsAndData);
        }

        private static async Task<string> GetResponseData(HttpResponseMessage response)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            keyValuePairs.Add(GetResponseStatusCodeDescription(response));
            keyValuePairs.AddRange(GetHeadersDescription(response.Headers));
            keyValuePairs.AddRange(GetResponseContentHeaders(response));
            keyValuePairs.Add(await GetResponseDataIfAny(response, keyValuePairs));

            return FormatHttpData(keyValuePairs);
        }

        private static KeyValuePair<string, string> GetRequestMethodDescription(HttpRequestMessage request)
        {
            return new KeyValuePair<string, string>("Method", $"{request.Method}, {request.RequestUri}");
        }

        private static async Task<IEnumerable<KeyValuePair<string, string>>> GetRequestContentHeadersDescriptionAndData(HttpRequestMessage request)
        {
            var contentDescriptionAndData = new List<KeyValuePair<string, string>>();
            var hasContent = request.Content != null && request.Content.IsMimeMultipartContent();

            if (!hasContent)
            {
                return contentDescriptionAndData;
            }

            var multipart = await request.Content.ReadAsMultipartAsync().ConfigureAwait(false);

            foreach (var httpContent in multipart.Contents)
            {
                if (httpContent.Headers.ContentType.MediaType != MediaType.ApplicationXml)
                {
                    continue;
                }

                contentDescriptionAndData.AddRange(SerializeHeaders(request.Content.Headers));
                contentDescriptionAndData.Add(new KeyValuePair<string, string>(null, $"{await GetContentData(httpContent).ConfigureAwait(false)}"));
            }

            return contentDescriptionAndData;
        }

        private static async Task<KeyValuePair<string, string>> GetResponseDataIfAny(HttpResponseMessage response, List<KeyValuePair<string, string>> keyValuePairs)
        {
            var hasContent = response.Content != null && response.Content.Headers.ContentType?.MediaType == MediaType.ApplicationXml;

            if (!hasContent) return new KeyValuePair<string, string>();

            return new KeyValuePair<string, string>(null, $"{await GetContentData(response.Content).ConfigureAwait(false)}");
        }

        private static KeyValuePair<string, string> GetResponseStatusCodeDescription(HttpResponseMessage response)
        {
            return new KeyValuePair<string, string>("StatusCode", $"{(int) response.StatusCode}, {response.StatusCode}");
        }

        private static List<KeyValuePair<string, string>> GetHeadersDescription(HttpHeaders headers)
        {
            return SerializeHeaders(headers);
        }

        private static IEnumerable<KeyValuePair<string, string>> GetResponseContentHeaders(HttpResponseMessage response)
        {
            return SerializeHeaders(response.Content.Headers);
        }

        private static List<KeyValuePair<string, string>> SerializeHeaders(HttpHeaders headers)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();

            keyValuePairs
                .AddRange(headers
                    .Select(header => new KeyValuePair<string, string>(header.Key, string.Join(",", header.Value))));

            return keyValuePairs;
        }

        private static string FormatHttpData(List<KeyValuePair<string, string>> keyValuePairs)
        {
            return keyValuePairs.Aggregate(System.Environment.NewLine, (current, keyValuePair) => current + $"{GetFormattedElement(keyValuePair)} {System.Environment.NewLine}");
        }

        private static string GetFormattedElement(KeyValuePair<string, string> keyValuePair)
        {
            var separator = !string.IsNullOrEmpty(keyValuePair.Key) ? ": " : string.Empty;

            return $"{keyValuePair.Key}{separator}{keyValuePair.Value}";
        }

        private static Task<string> GetContentData(HttpContent httpContent)
        {
            return httpContent?.ReadAsStringAsync() ?? Task.FromResult(string.Empty);
        }
    }
}