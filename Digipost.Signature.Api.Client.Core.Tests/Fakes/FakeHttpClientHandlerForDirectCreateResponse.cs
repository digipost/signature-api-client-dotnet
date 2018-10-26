using System.Net.Http;
using System.Net.Http.Headers;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    internal class FakeHttpClientHandlerForDirectCreateResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            var stringContent = new StringContent(ContentUtility.GetCreateResponse());
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            return stringContent;
        }
    }
}
