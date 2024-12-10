using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;
using Digipost.Api.Client.Shared.Resources.Resource;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Digipost.Signature.Api.Client.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Extensions.Logging;

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
            return new ClientConfiguration(Environment.DifiQa, GetBringCertificate(), GetSender())
            {
                CertificateValidationPreferences = {ValidateSenderCertificate = false}
            };
        }

        public static Document GetDocument()
        {
            return new DocumentStub("Testdocument", FileType.Pdf, GetPdfDocumentBytes());
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
            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();
            return CertificateReader.ReadCertificate(serviceProvider.GetService<ILoggerFactory>());
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

        private static X509Certificate2 EternalTestCertificateWithPrivateKey()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes("Certificates", "Unittests", "DigipostCert.p12"), "qwer1234", X509KeyStorageFlags.Exportable);
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
