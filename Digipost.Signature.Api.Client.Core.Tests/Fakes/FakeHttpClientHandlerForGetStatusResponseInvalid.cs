using System.Net.Http;
using System.Text;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    internal class FakeHttpClientHandlerForStatusResponseInvalid : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return new StringContent(ContentUtility.GetStatusResponseInvalid(), Encoding.UTF8, "application/xml");
        }
    }
}