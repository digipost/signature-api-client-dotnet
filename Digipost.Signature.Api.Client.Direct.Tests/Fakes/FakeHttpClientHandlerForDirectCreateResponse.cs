using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Direct.Tests.Fakes
{
    internal class FakeHttpClientHandlerForDirectCreateResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return new StringContent(ResponseUtility.GetCreateResponse());
        }
    }
}
