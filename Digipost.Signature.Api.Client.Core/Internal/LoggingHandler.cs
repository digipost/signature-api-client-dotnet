using System.Collections.Generic;
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
        private static readonly ILog Log = LogManager.GetLogger("Digipost.Signature.Api.Client.RequestLogger");

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (Log.IsDebugEnabled)
            {
                await LogRequest(request);
            }

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            if (Log.IsDebugEnabled)
            {
                await LogResponse(httpResponseMessage);
            }

            return httpResponseMessage;
        }

        private static async Task LogRequest(HttpRequestMessage request)
        {
            var requestData = await GetRequestData(request);
            Log.Debug($"Outgoing: {requestData}");
        }

        private static async Task<string> GetRequestData(HttpRequestMessage request)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Method", $"{request.Method}, {request.RequestUri}")
            };

            keyValuePairs.AddRange(SerializeHeaders(request.Headers));

            if (request.Content != null && request.Content.IsMimeMultipartContent())
            {
                var multipart = await request.Content.ReadAsMultipartAsync();
                
                foreach (var httpContent in multipart.Contents)
                {
                    if (httpContent.Headers.ContentType.MediaType == MediaType.ApplicationXml)
                    {
                        keyValuePairs.AddRange(SerializeHeaders(request.Content.Headers));
                        keyValuePairs.Add(new KeyValuePair<string, string>(null, $"{await GetContentData(httpContent)}"));
                    }
                }
            }

            return FormatHttpData(keyValuePairs);
        }

        private static async Task LogResponse(HttpResponseMessage response)
        {
            var responseData = await GetResponseData(response);
            Log.Debug($"Incoming: {responseData}");
        }

        private static async Task<string> GetResponseData(HttpResponseMessage response)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("StatusCode", $"{(int) response.StatusCode}, {response.StatusCode}")
            };

            keyValuePairs.AddRange(SerializeHeaders(response.Headers));
            keyValuePairs.AddRange(SerializeHeaders(response.Content.Headers));
            keyValuePairs.Add(new KeyValuePair<string, string>(null, $"{await GetContentData(response.Content)}"));

            return FormatHttpData(keyValuePairs);
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
            return httpContent != null ? httpContent.ReadAsStringAsync() : Task.FromResult(string.Empty);
        }
    }
}