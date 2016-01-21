using System.Net.Http;

namespace Digipost.Signature.Api.Client.Direct.Tests.Fakes
{
    class FakeHttpClientHandlerForDirectCreateResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return new StringContent(
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<direct-signature-job-response xmlns=\"http://signering.posten.no/schema/v1\" xmlns:ns2=\"http://uri.etsi.org/01903/v1.3.2#\" xmlns:ns3=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ns4=\"http://uri.etsi.org/2918/v1.2.1#\">" +
                    "<signature-job-id>55</signature-job-id>" +
                    "<redirect-url>https://localhost:9000/redirect/#/713ebf74a6ff1ddff16482e4f67df8c6f903c5f5f4aec05913edcafb57560aba</redirect-url>" +
                    "<status-url>https://localhost:8443/api/signature-jobs/55/status</status-url>" +
                "</direct-signature-job-response>");
        }
    }
}
