using System.Net;
using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;

namespace Digipost.Signature.Api.Client.Portal.Tests.Fakes
{
    internal class FakeHttpClientHandlerForJobCompletedResponse : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHandlerForJobCompletedResponse()
        {
            ResultCode = HttpStatusCode.Conflict;
        }

        public override HttpContent GetContent()
        {
            return new StringContent("");
        }
    }
}