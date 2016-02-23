using System.Net;
using System.Net.Http;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Portal.Tests.Fakes
{
    public class FakeHttpClientHandlerForEmptyQueueResponse : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHandlerForEmptyQueueResponse()
        {
            ResultCode = HttpStatusCode.NoContent;
        }

        public override HttpContent GetContent()
        {
            return new StringContent(ResponseUtility.GetEmptyQueueContent());
        }
    }
}