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
            var serviceProvider = CreateServiceProvider();
            SetUpNLog(serviceProvider);

            var certificateReader2 = new CertificateReader2(serviceProvider.GetService<ILoggerFactory>());
            return certificateReader2.ReadCertificate();
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
        
        private static void SetUpNLog(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true});
            NLog.LogManager.LoadConfiguration("/Users/aas/projects/digipost/signature-api-client-dotnet/Digipost.Signature.Api.Client.Program/nlog.config");
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            return services.BuildServiceProvider();
        }
    }
}
