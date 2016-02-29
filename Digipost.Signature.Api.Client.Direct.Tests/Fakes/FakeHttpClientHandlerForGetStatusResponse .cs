using System.Net.Http;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Direct.Tests.Fakes
{
    internal class FakeHttpClientHandlerGetStatusResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return new StringContent(ResponseUtility.GetStatusResponse());
        }
    }
}