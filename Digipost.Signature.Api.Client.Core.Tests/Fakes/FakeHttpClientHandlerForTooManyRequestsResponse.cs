using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Xunit.Sdk;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    public class FakeHttpClientHandlerForTooManyRequestsResponse : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHandlerForTooManyRequestsResponse()
        {
            ResultCode = (HttpStatusCode?) 429;
            AddNextPermittedPollTimeHeader(DateTime.Now.AddSeconds(30));
        }

        public override HttpContent GetContent()
        {
            return new StringContent(CoreResponseUtility.GetTooManyRequestsResponse());
        }
    }
}
