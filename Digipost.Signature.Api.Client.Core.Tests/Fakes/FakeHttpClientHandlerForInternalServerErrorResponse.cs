using System.Net;
using System.Net.Http;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    internal class FakeHttpClientHandlerForInternalServerErrorResponse : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHandlerForInternalServerErrorResponse()
        {
            ResultCode = HttpStatusCode.InternalServerError;
        }

        public override HttpContent GetContent()
        {
            return new StringContent("");
        }
    }
}
