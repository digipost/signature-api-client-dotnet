using System;
using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Portal.Tests.Fakes
{
    public class FakeHttpClientHandlerForJobStatusChangeResponse : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHandlerForJobStatusChangeResponse()
        {
            AddNextPermittedPollTimeHeader(DateTime.Now.AddSeconds(30));
        }
        
        public override HttpContent GetContent()
        {
            return new StringContent(ResponseUtility.GetJobStatusChangeResponse());
        }
    }
}
