using System.Net;
using System.Net.Http;
using Digipost.Signature.Api.Client.Portal.Tests.Fakes;

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