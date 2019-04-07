using System;
using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Direct.Tests.Fakes
{
    internal class FakeHttpClientHandlerGetStatusResponse : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHandlerGetStatusResponse()
        {
            AddNextPermittedPollTimeHeader(DateTime.Now.AddSeconds(30));
        }
        
        public override HttpContent GetContent()
        {
            return new StringContent(ResponseUtility.GetStatusResponse());
        }
    }
}
