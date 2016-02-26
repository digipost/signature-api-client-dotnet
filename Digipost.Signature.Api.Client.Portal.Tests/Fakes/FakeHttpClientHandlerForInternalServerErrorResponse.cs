using System.Net;
using System.Net.Http;

namespace Digipost.Signature.Api.Client.Portal.Tests.Fakes
{
    internal class FakeHttpClientHandlerForInternalServerErrorResponse : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHandlerForInternalServerErrorResponse()
        {
            ResultCode = HttpStatusCode.BadRequest;
        }

        public override HttpContent GetContent()
        {
            return new StringContent("");
        }
    }
}