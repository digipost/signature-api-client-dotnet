using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    internal class FakeHttpClientForPadesResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return new StreamContent(CoreResponseUtility.GetPades());
        }
    }
}