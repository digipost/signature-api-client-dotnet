using System.Net.Http;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Portal.Tests.Fakes
{
    public class FakeHttpClientHandlerForJobStatusChangeResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return new StringContent(ResponseUtility.GetJobStatusChangeResponse());
        }
    }
}
