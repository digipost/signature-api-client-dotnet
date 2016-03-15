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
            var keyValuePairs = new List<string[]>
            {
                new[] {"Method", $"{request.Method}, {request.RequestUri}"},
                new[] {"Accept", $"{request.Headers.Accept}"},
                new[] {"UserAgent", $"{request.Headers.UserAgent}"},
                new[] {"", $"{await GetContentData(request.Content)}"}
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
            var keyValuePairs = new List<string[]>
            {
                new[] {"StatusCode", $"{(int) response.StatusCode}, {response.StatusCode}"},
                new[] {"Cache-Control", $"{response.Headers.CacheControl}"},
                new[] {"Content-Length", $"{response.Content.Headers.ContentLength}"},
                new[] {"Content-Type", $"{response.Content.Headers.ContentType}"},
                new[] {"Date", $"{response.Headers.Date}"},
                new[] {"", $"{await GetContentData(response.Content)}"}
            };

            return FormatHttpData(keyValuePairs);
        }

        private static string FormatHttpData(IEnumerable<string[]> keyValuePairs)
        {
            return keyValuePairs.Aggregate(System.Environment.NewLine, (current, keyValuePair) => current + $"{keyValuePair.ElementAt(0)}:  {keyValuePair.ElementAt(1)} {System.Environment.NewLine}");
        }

        private static async Task<string> GetContentData(HttpContent httpContent)
        {
            var requestContent = "";
            if (httpContent != null)
            {
                requestContent = await httpContent.ReadAsStringAsync();
            }

            return requestContent;
        }
    }
}