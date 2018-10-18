using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;

namespace Digipost.Signature.Api.Client.Portal.Tests.Fakes
{
    internal class FakeHttpClientHandlerChecksCorrectSenderResponse : FakeHttpClientHandlerResponse
    {
        public string RequestUri { get; internal set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri.ToString();
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        public override HttpContent GetContent()
        {
            return new StringContent(ResponseUtility.GetJobStatusChangeResponse());
        }
    }
}
