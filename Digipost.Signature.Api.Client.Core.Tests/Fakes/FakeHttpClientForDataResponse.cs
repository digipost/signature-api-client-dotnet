using System.Net.Http;
using Digipost.Signature.Api.Client.Portal.Tests.Fakes;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    internal class FakeHttpClientForDataResponse :FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return new StringContent("Some string content");
        }
    }
}
