using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal class LoggingHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger("Digipost.Signature.Api.Client.RequestLogger");

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
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
                new KeyValuePair<string, string>("Method", $"{request.Method}, {request.RequestUri}"),
                new KeyValuePair<string, string>("Accept", $"{request.Headers.Accept}"),
                new KeyValuePair<string, string>("UserAgent", $"{request.Headers.UserAgent}"),
                new KeyValuePair<string, string>(null, $"{await GetContentData(request.Content)}")
            };

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
                new KeyValuePair<string, string>("StatusCode", $"{(int) response.StatusCode}, {response.StatusCode}"),
                new KeyValuePair<string, string>("Cache-Control", $"{response.Headers.CacheControl}"),
                new KeyValuePair<string, string>("Content-Length", $"{response.Content.Headers.ContentLength}"),
                new KeyValuePair<string, string>("Content-Type", $"{response.Content.Headers.ContentType}"),
                new KeyValuePair<string, string>("Date", $"{response.Headers.Date}"),
                new KeyValuePair<string, string>(null, $"{await GetContentData(response.Content)}")
            };

            return FormatHttpData(keyValuePairs);
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

        private static async Task<string> GetContentData(HttpContent httpContent)
        {
            var requestContent = string.Empty;
            if (httpContent != null)
            {
                requestContent = await httpContent.ReadAsStringAsync();
            }

            return requestContent;
        }
    }
}