using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Internal.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using static Digipost.Signature.Api.Client.Direct.Enums.StatusRetrievalMethod;

namespace Digipost.Signature.Api.Client.Direct.Tests.Utilities
{
    public class DomainUtility
    {
        public static Job GetDirectJob()
        {
            return new Job("Job title", GetDirectDocuments(), GetSigner(), "Reference", GetExitUrls());
        }

        public static Job GetDirectJob(params SignerIdentifier[] signers)
        {
            return new Job("Job title", GetDirectDocuments(), signers.Select(pin => new Signer(pin) {SignatureType = SignatureType.AuthenticatedSignature}), "Reference", GetExitUrls());
        }

        public static Job GetDirectJobWithSender()
        {
            return new Job("Job title", GetDirectDocuments(), GetSigner(), "Reference", GetExitUrls(), CoreDomainUtility.GetSender());
        }

        public static Job GetPollableDirectJob(Sender sender, params SignerIdentifier[] signers)
        {
            return new Job("Job title", GetDirectDocuments(), signers.Select(pin => new Signer(pin)), "Reference", GetExitUrls(), sender, Polling);
        }

        internal static Manifest GetDirectManifest()
        {
            return new Manifest(
                "Job title", 
                CoreDomainUtility.GetSender(),
                GetDirectDocuments(),
                GetSigner()
            );
        }

        internal static SignatureGenerator GetSignature()
        {
            return new SignatureGenerator(CoreDomainUtility.GetTestCertificate(), GetDirectDocuments(), GetDirectManifest());
        }

        public static JobStatusResponse GetJobStatusResponse()
        {
            var jobId = 22;
            var jobReference = "senders-reference";
            var jobStatus = JobStatus.Failed;
            var statusResponseUrls = GetJobReferences();
            var nextPermittedPollTime = DateTime.Now;

            return new JobStatusResponse(
                jobId,
                jobReference,
                jobStatus,
                statusResponseUrls,
                new List<Signature> {new Signature("12345678910", null, SignatureStatus.Failed, DateTime.Now)},
                nextPermittedPollTime
            );
        }

        public static List<SignerResponse> GetResponseSigners()
        {
            return new List<SignerResponse>()
            {
                new SignerResponse(
                    new PersonalIdentificationNumber("12345678910"),
                    new Uri("http://responseurl.no"),
                    NewRedirectUrlRequest.FromSignerUrl(new Uri("http://signerurl.no"))
                )
            };
        }

        public static ExitUrls GetExitUrls()
        {
            var completionUrl = new Uri("http://localhost/completion");
            var cancellationUrl = new Uri("http://localhost/cancellation");
            var errorUrl = new Uri("http://localhost/error");

            return new ExitUrls(completionUrl, cancellationUrl, errorUrl);
        }

        public static Uri GetSignatureResponseObjects()
        {
            return new Uri("http://signatureServiceRoot.Digipost.no");
        }

        public static JobResponse GetDirectJobResponse()
        {
            var jobId = 123456789;
            var jobReference = "senders-reference";
            var signers = GetResponseSigners();
            var statusUrl = new StatusUrl(new Uri("http://statusurl.no"));

            return new JobResponse(
                jobId,
                jobReference,
                signers,
                statusUrl
            );
        }

        public static JobReferences GetJobReferences()
        {
            return new JobReferences(
                new Uri("http://signatureRoot.digipost.no/confirmation"),
                new Uri("http://signatureRoot.digipost.no/pades"));
        }

        public static List<Signature> GetSignatures(int count)
        {
            if (count > 9)
            {
                throw new ArgumentException("Maximum of 9 senders.");
            }

            var signatures = new List<Signature>();

            const string basePersonalIdentificationNumber = "0101330000";
            for (var i = 1; i <= count; i++)
            {
                signatures.Add(new Signature(basePersonalIdentificationNumber + i, new XadesReference(new Uri("https://signatureRoot.digipost.no/xades")), SignatureStatus.Signed, DateTime.Now));
            }

            return signatures;
        }

        public static Document GetDirectDocument()
        {
            return new Document("TheTitle", FileType.Pdf, CoreDomainUtility.GetPdfDocumentBytes());
        }

        public static List<Document> GetDirectDocuments()
        {
            return new List<Document>{ GetDirectDocument() };
        }

        public static List<Signer> GetSigner()
        {
            return GetSigners(1);
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
                signers.Add(new Signer(new PersonalIdentificationNumber(basePersonalIdentificationNumber + i)));
            }

            return signers;
        }
    }
}
