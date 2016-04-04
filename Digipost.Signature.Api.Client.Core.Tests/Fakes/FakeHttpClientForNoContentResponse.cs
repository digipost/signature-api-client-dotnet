using System.Net.Http;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    internal class FakeHttpClientHandlerForNoContentResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return null;
        }
    }
}