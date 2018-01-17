using System;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;
using Digipost.Api.Client.Shared.Resources.Resource;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;

namespace Digipost.Signature.Api.Client.Core.Tests.Utilities
{
    public static class CoreDomainUtility
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "Digipost.Signature.Api.Client.Core.Tests.Resources");

        public static string BringPublicOrganizationNumber => "988015814";

        public static string BringPrivateOrganizationNumber => "088015814";

        public static string PostenOrganizationNumber => "984661185";

        public static ClientConfiguration GetClientConfiguration()
        {
            return new ClientConfiguration(Environment.DifiQa, GetBringCertificate(), GetSender());
        }

        public static AbstractDocument GetDocument()
        {
            return new DocumentStub("Testdocument", "A test document from domain Utility", FileType.Pdf, GetPdfDocumentBytes());
        }

        public static Sender GetSender()
        {
            var organizationNumberQaOrganization = BringPublicOrganizationNumber;
            return new Sender(organizationNumberQaOrganization);
        }

        public static byte[] GetPdfDocumentBytes()
        {
            return ResourceUtility.ReadAllBytes("Documents", "Dokument.pdf");
        }

        public static X509Certificate2 GetTestCertificate()
        {
            return EternalTestCertificateWithPrivateKey();
        }

        public static X509Certificate2 GetBringCertificate()
        {
            return BringTestSertifikat();
        }

        public static X509Certificate2 GetExpiredSelfSignedCertificate()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes("Certificates", "Unittests", "ExpiredTestCertificate.cer"), "", X509KeyStorageFlags.Exportable);
        }

        public static X509Certificate2 GetNotActivatedSelfSignedCertificate()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes("Certificates", "Unittests", "NotActivatedTestCertificate.cer"), "", X509KeyStorageFlags.Exportable);
        }

        public static X509Certificate2 GetPostenTestCertificate()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes("Certificates", "Unittests", "PostenNorgeAs.cer"), "", X509KeyStorageFlags.Exportable);
        }

        private static X509Certificate2 BringTestSertifikat()
        {
            return CertificateUtility.SenderCertificate("2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed");
        }

        private static X509Certificate2 EternalTestCertificateWithPrivateKey()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes("Certificates", "Unittests", "DigipostCert.p12"), "", X509KeyStorageFlags.Exportable);
        }

        public static HttpClient GetHttpClientWithHandler(DelegatingHandler delegatingHandler)
        {
            return new HttpClient(delegatingHandler)
            {
                BaseAddress = new Uri("http://mockUrl.no")
            };
        }
    }
}