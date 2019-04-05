using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    public class FakeHttpClientHandlerForEmptyQueueResponse : FakeHttpClientHandlerResponse
    {
        public DateTime NextPermittedPollTime = DateTime.Now.AddSeconds(30);

        public FakeHttpClientHandlerForEmptyQueueResponse()
        {
            ResultCode = HttpStatusCode.NoContent;
            HttpResponseHeader = new KeyValuePair<string, string>("X-Next-permitted-poll-time", NextPermittedPollTime.ToString("O"));
        }

        public override HttpContent GetContent()
        {
            return new StringContent(CoreResponseUtility.GetEmptyQueueContent());
        }
    }
}
