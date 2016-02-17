using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Signature.Api.Client.Portal.Tests.Fakes
{
    public abstract class FakeHttpClientHandlerResponse : DelegatingHandler
    {
        public HttpStatusCode? ResultCode { get; set; }

        public HttpContent HttpContent { get; set; }

        public KeyValuePair<string,string> HttpResponseHeader { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(
           HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = HttpContent ?? GetContent(),
                StatusCode = ResultCode ?? HttpStatusCode.OK
            };
            AddResponseHeader(response);

            return await Task.FromResult(response);
        }

        private void AddResponseHeader(HttpResponseMessage response)
        {
            if (HttpResponseHeader.Key != null)
            {
                response.Headers.Add(HttpResponseHeader.Key, HttpResponseHeader.Value);
            }
        }

        public abstract HttpContent GetContent();
    }
}
