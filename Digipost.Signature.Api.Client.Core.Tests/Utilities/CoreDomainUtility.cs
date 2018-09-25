using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;
using Digipost.Api.Client.Shared.Resources.Resource;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Newtonsoft.Json;

namespace Digipost.Signature.Api.Client.Core.Tests.Utilities
{
    public static class CoreDomainUtility
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "Digipost.Signature.Api.Client.Core.Tests.Resources");

        public static string BringPublicOrganizationNumber => "984661185";

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
            return DIGIPOSTPRODSERTIFIKAT();
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

        //Todo: Should not manipulate certificate store like this.
        private static X509Certificate2 BringTestSertifikat()
        {
            var pathToSecretsWithPlaceholder = "<PATH-PLACEHOLDER>/.microsoft/usersecrets/User-Secret-ID/secrets.json";
            var replace = pathToSecretsWithPlaceholder.Replace("<PATH-PLACEHOLDER>", System.Environment.GetEnvironmentVariable("HOME"));
            Console.WriteLine("AAS Reading test certificate from secrets file: " + replace);
            string value = File.ReadAllText(replace);
            var deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);

            string certificatePath;
            deserializeObject.TryGetValue("Certificate:Path:Absolute", out certificatePath);
            string certificatePassword;
            deserializeObject.TryGetValue("Certificate:Password", out certificatePassword);
            
            Console.WriteLine("AAS Reading test certificate from path found in secrets file: " + certificatePath);

            var bringTestSertifikat = new X509Certificate2(certificatePath, certificatePassword, X509KeyStorageFlags.Exportable);
            return bringTestSertifikat;
        }
        
        //Todo: Should not manipulate certificate store like this.
        private static X509Certificate2 DIGIPOSTPRODSERTIFIKAT()
        {
            var pathToSecretsWithPlaceholder = "<PATH-PLACEHOLDER>/.microsoft/usersecrets/User-Secret-ID/secrets.json";
            var replace = pathToSecretsWithPlaceholder.Replace("<PATH-PLACEHOLDER>", System.Environment.GetEnvironmentVariable("HOME"));
            Console.WriteLine("AAS Reading test certificate from secrets file: " + replace);
            string value = File.ReadAllText(replace);
            var deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);

            string certificatePath;
            deserializeObject.TryGetValue("Certificate:Path:Absolute", out certificatePath);
            string certificatePassword;
            deserializeObject.TryGetValue("Certificate:Password", out certificatePassword);
            
            Console.WriteLine("AAS Reading test certificate from path found in secrets file: " + certificatePath);

            var x509Certificate2 = new X509Certificate2(certificatePath, certificatePassword, X509KeyStorageFlags.Exportable);
            return x509Certificate2;
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