using System.Net.Http;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    internal class FakeHttpClientForDataResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return new StringContent("Some string content");
        }
    }
}
