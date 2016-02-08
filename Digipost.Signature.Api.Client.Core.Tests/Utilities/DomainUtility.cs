using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Direct;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Portal;

namespace Digipost.Signature.Api.Client.Core.Tests.Utilities
{
    public static class DomainUtility
    {
        static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Signature.Api.Client.Core.Tests.Resources");

        internal static SignatureGenerator GetSignature()
        {
            return new SignatureGenerator(GetTestCertificate(), GetDocument(), GetManifest());
        } 

        public static Manifest GetManifest()
        {
            return new Manifest(
                GetSender(),
                GetDocument(),
                GetSigners(2)
                );
        }

        public static ClientConfiguration GetClientConfiguration()
        {
            return new ClientConfiguration(new Uri("https://serviceroot.digipost.no"), GetSender(), GetTestCertificate());
        }

        public static DirectJob GetDirectJob()
        {
            return new DirectJob(GetDocument(), GetSigner(), "Reference", GetExitUrls());
        }

        public static PortalJob GetPortalJob()
        {
            return new PortalJob(GetDocument(), GetSigners(3), "PortalJobReference");
        }

        public static Document GetDocument()
        {
            return new Document("Testdocument", "A test document from domain Utility", "TestFileName.pdf", FileType.Pdf, GetPdfDocumentBytes());
        }

        public static Sender GetSender()
        {
            var organizationNumberQaOrganization = "983163327";
            return new Sender(organizationNumberQaOrganization);
        }

        public static Signer GetSigner()
        {
            return GetSigners(1).First();
        }

        public static List<Signer> GetSigners(int count)
        {
            if (count > 9)
            {
                throw new ArgumentException("Maximum of 9 senders.");
            }

            var signers = new List<Signer>();

            const string basePersonalIdentificationNumber = "0101330000";
            for (var i = 1; i <= count; i++)
            {
                signers.Add(new Signer(basePersonalIdentificationNumber + i));
            }

            return signers;
        }

        public static byte[] GetPdfDocumentBytes()
        {
            return ResourceUtility.ReadAllBytes(true, "Documents", "Dokument.pdf");
        }

        public static X509Certificate2 GetTestCertificate()
        {
            return EternalTestCertificateWithPrivateKey();
        }

        public static X509Certificate2 GetTestIntegrasjonSertifikat()
        {
            return BringTestSertifikat();
        }

        private static X509Certificate2 BringTestSertifikat()
        {
            return CertificateUtility.SenderCertificate("2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed", Language.English);
        }

        private static X509Certificate2 EternalTestCertificateWithPrivateKey()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Certificates", "Unittests", "DigipostCert.p12"), password: "", keyStorageFlags: X509KeyStorageFlags.Exportable);
        }

        public static DirectJobStatusResponse GetDirectJobStatusResponse()
        {
            //Arrange
            var jobId = 22;
            var jobStatus = JobStatus.Cancelled;
            var statusResponseUrls = GetJobReferences();

            //Act
             return new DirectJobStatusResponse(
                jobId,
                jobStatus,
                statusResponseUrls
            );
        }

        public static ResponseUrls GetResponseUrls()
        {
            //Arrange
            var redirectUrl = new Uri("http://responseurl.no");
            var statusUrl = new Uri("http://statusurl.no");


            //Act
            return new ResponseUrls(
                redirectUrl,
                statusUrl
                );
        }

        public static ExitUrls GetExitUrls()
        {
            var completionUrl = new Uri("http://localhost/completion");
            var cancellationUrl = new Uri("http://localhost/cancellation");
            var errorUrl = new Uri("http://localhost/error");

            return new ExitUrls(completionUrl, cancellationUrl, errorUrl);
        }

        public static Uri GetSignatureResponseObjects(){
            return new Uri("http://signatureServiceRoot.Digipost.no");
        }

        public static DirectJobResponse GetDirectJobResponse()
        {
            var jobId = 123456789;
            var responseUrls = GetResponseUrls();

            return new DirectJobResponse(
                jobId,
                responseUrls
                );
        }

        public static JobReferences GetJobReferences()
        {
            return new JobReferences(
                new Uri("http://signatureRoot.digipost.no/confirmation"),
                new Uri("http://signatureRoot.digipost.no/xades"),
                new Uri("http://signatureRoot.digipost.no/pades"));
        }
    }
}
