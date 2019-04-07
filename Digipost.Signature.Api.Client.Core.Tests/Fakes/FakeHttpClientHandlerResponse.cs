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
        public HttpStatusCode? ResultCode { get; protected set; }

        public DateTime NextPermittedPollTime { get; private set; }
        
        private HttpContent HttpContent { get; set; }

        private KeyValuePair<string, string> HttpResponseHeader { get; set; }


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

        protected void AddNextPermittedPollTimeHeader(DateTime nextPermittedPollTime)
        {
            NextPermittedPollTime = nextPermittedPollTime;
            HttpResponseHeader = new KeyValuePair<string, string>
            (
                BaseClient.NextPermittedPollTimeHeader,
                nextPermittedPollTime.ToString("O")
            );
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
