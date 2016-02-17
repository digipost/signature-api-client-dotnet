using System.Net;
using System.Net.Http;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Portal.Tests.Fakes
{
    class FakeHttpClientHanderForBrokerNotAuthorizedErrorResponse : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHanderForBrokerNotAuthorizedErrorResponse()
        {
            ResultCode = HttpStatusCode.BadRequest;
        }

        public override HttpContent GetContent()
        {
            return new StringContent(ResponseUtility.GetBrokerNotAuthorizedError());
        }
    }
}
