using System.Net;
using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Tests.Fakes;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    internal class FakeHttpClientHandlerForErrorResponse : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHandlerForErrorResponse()
        {
            ResultCode = HttpStatusCode.BadRequest;
        }

        public override HttpContent GetContent()
        {
            return new StringContent(CoreResponseUtility.GetError());
        }
    }
}