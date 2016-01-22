using System.Net.Http;

namespace Digipost.Signature.Api.Client.Direct.Tests.Fakes
{
    class FakeHttpClientHandlerGetStatusResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return new StringContent(
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<direct-signature-job-status-response xmlns=\"http://signering.posten.no/schema/v1\" xmlns:ns2=\"http://uri.etsi.org/01903/v1.3.2#\" xmlns:ns3=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ns4=\"http://uri.etsi.org/2918/v1.2.1#\">" +
                    "<signature-job-id>102</signature-job-id>" +
                    "<status>SIGNED</status>" +
                    "<confirmation-url>https://172.16.91.1:8443/api/signature-jobs/102/complete</confirmation-url>" +
                    "<xades-url>https://172.16.91.1:8443/api/signature-jobs/102/xades/107</xades-url>" +
                    "<pades-url>https://172.16.91.1:8443/api/signature-jobs/102/pades</pades-url>" +
                "</direct-signature-job-status-response>"
                );
        }
    }
}
