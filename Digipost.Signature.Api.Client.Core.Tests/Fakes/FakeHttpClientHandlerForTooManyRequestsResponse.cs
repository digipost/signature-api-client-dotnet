using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    public class FakeHttpClientHandlerForTooManyRequestsResponse : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHandlerForTooManyRequestsResponse()
        {
            ResultCode = (HttpStatusCode?) 429;
            HttpResponseHeader = new KeyValuePair<string, string>("X-Next-permitted-poll-time", DateTime.Now.ToString("O"));
        }

        public override HttpContent GetContent()
        {
            return new StringContent(CoreResponseUtility.GetTooManyRequestsResponse());
        }
    }
}