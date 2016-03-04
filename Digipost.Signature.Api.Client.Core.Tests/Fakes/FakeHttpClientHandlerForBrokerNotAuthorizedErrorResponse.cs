using System.Net;
using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Portal.Tests.Fakes
{
    internal class FakeHttpClientHanderForBrokerNotAuthorizedErrorResponse : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHanderForBrokerNotAuthorizedErrorResponse()
        {
            ResultCode = HttpStatusCode.BadRequest;
        }

        public override HttpContent GetContent()
        {
            return new StringContent(CoreResponseUtility.GetBrokerNotAuthorizedError());
        }
    }
}