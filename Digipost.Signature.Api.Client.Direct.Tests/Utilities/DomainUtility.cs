using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Direct.Tests.Utilities
{
    public class DomainUtility
    {
        public static Job GetDirectJob()
        {
            return new Job(GetDirectDocument(), GetSigner(), "Reference", GetExitUrls());
        }

        public static Job GetDirectJob(params SignerIdentifier[] signers)
        {
            return new Job(GetDirectDocument(), signers.Select(pin => new Signer(pin) {SignatureType = SignatureType.AdvancedSignature }), "Reference", GetExitUrls());
        }

        public static Job GetDirectJobWithSender()
        {
            return new Job(GetDirectDocument(), GetSigner(), "Reference", GetExitUrls(), CoreDomainUtility.GetSender());
        }

        public static Job GetPollableDirectJob(params SignerIdentifier[] signers)
        {
            return new Job(GetDirectDocument(), signers.Select(pin => new Signer(pin)), "Reference", GetExitUrls(), statusRetrievalMethod: StatusRetrievalMethod.Polling);
        }

        internal static Manifest GetDirectManifest()
        {
            return new Manifest(
                CoreDomainUtility.GetSender(),
                GetDirectDocument(),
                GetSigner()
            );
        }

        internal static SignatureGenerator GetSignature()
        {
            return new SignatureGenerator(CoreDomainUtility.GetTestCertificate(), GetDirectDocument(), GetDirectManifest());
        }

        public static JobStatusResponse GetJobStatusResponse()
        {
            var jobId = 22;
            var jobStatus = JobStatus.Failed;
            var statusResponseUrls = GetJobReferences();

            return new JobStatusResponse(
                jobId,
                jobStatus,
                statusResponseUrls,
                new List<Signature> {new Signature(new PersonalIdentificationNumber("12345678910"), null, SignatureStatus.Failed, DateTime.Now)}
            );
        }

        public static ResponseUrls GetResponseUrls()
        {
            var redirectUrl = new List<RedirectReference> {new RedirectReference(new Uri("http://responseurl.no"), new PersonalIdentificationNumber("12345678910"))};
            var statusUrl = new Uri("http://statusurl.no");

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

        public static Uri GetSignatureResponseObjects()
        {
            return new Uri("http://signatureServiceRoot.Digipost.no");
        }

        public static JobResponse GetDirectJobResponse()
        {
            var jobId = 123456789;
            var responseUrls = GetResponseUrls();

            return new JobResponse(
                jobId,
                responseUrls
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
                signatures.Add(new Signature(new PersonalIdentificationNumber(basePersonalIdentificationNumber + i), new XadesReference(new Uri("https://signatureRoot.digipost.no/xades")), SignatureStatus.Signed, DateTime.Now));
            }

            return signatures;
        }

        public static Document GetDirectDocument()
        {
            return new Document("TheTitle", "The direct document message", FileType.Pdf, CoreDomainUtility.GetPdfDocumentBytes());
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