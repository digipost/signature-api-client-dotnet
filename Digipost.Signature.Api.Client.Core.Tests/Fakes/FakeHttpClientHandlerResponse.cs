using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    public abstract class FakeHttpClientHandlerResponse : DelegatingHandler
    {
        public HttpStatusCode? ResultCode { get; set; }

        public HttpContent HttpContent { get; set; }

        public KeyValuePair<string, string> HttpResponseHeader { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage
            {
                Content = HttpContent ?? GetContent(),
                StatusCode = ResultCode ?? HttpStatusCode.OK,
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri("http://fake.uri.for.fake.httpClientHandlerResponse.inTests.no"))
            };
            AddResponseHeader(response);

            return await Task.FromResult(response).ConfigureAwait(false);
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
