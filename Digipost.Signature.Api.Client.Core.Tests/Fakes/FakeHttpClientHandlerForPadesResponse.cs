using System.IO;
using System.Net.Http;
using System.Text;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    public class FakeHttpClientHandlerForPadesResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("Manuel er kul"));
            return new StreamContent(stream);
        }
    }
}
