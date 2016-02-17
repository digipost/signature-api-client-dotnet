using System;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Direct.Tests.Utilities
{
    public class DomainUtility
    {
        public static DirectJob GetDirectJob()
        {
            return new DirectJob(CoreDomainUtility.GetDocument(), CoreDomainUtility.GetSigner(), "Reference", GetExitUrls());
        }

        internal static DirectManifest GetDirectManifest()
        {
            return new DirectManifest(
                CoreDomainUtility.GetSender(),
                CoreDomainUtility.GetDocument(),
                CoreDomainUtility.GetSigner()
                );
        }

        internal static SignatureGenerator GetSignature()
        {
            return new SignatureGenerator(CoreDomainUtility.GetTestCertificate(), CoreDomainUtility.GetDocument(), GetDirectManifest());
        }

        public static DirectJobStatusResponse GetDirectJobStatusResponse()
        {
            var jobId = 22;
            var jobStatus = JobStatus.Cancelled;
            var statusResponseUrls = GetJobReferences();

            return new DirectJobStatusResponse(
                jobId,
                jobStatus,
                statusResponseUrls
            );
        }

        public static ResponseUrls GetResponseUrls()
        {
            var redirectUrl = new Uri("http://responseurl.no");
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
