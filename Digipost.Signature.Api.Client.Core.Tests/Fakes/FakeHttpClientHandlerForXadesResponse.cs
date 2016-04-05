using System.Net.Http;
using System.Net.Http.Headers;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    internal class FakeHttpClientHandlerForXadesResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            var streamContent = new StreamContent(CoreResponseUtility.GetXades());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            return streamContent;
        }
    }
}