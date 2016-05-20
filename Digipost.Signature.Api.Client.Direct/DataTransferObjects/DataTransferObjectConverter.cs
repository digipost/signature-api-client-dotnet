using System;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Extensions;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Direct.DataTransferObjects
{
    public static class DataTransferObjectConverter

    {
        public static directsignaturejobrequest ToDataTransferObject(Job job)
        {
            return new directsignaturejobrequest
            {
                reference = job.Reference,
                exiturls = ToDataTransferObject(job.ExitUrls)
            };
        }

        public static exiturls ToDataTransferObject(ExitUrls exitUrls)
        {
            return new exiturls
            {
                completionurl = exitUrls.CompletionUrl.ToString(),
                errorurl = exitUrls.ErrorUrl.ToString(),
                rejectionurl = exitUrls.RejectionUrl.ToString()
            };
        }

        public static JobResponse FromDataTransferObject(directsignaturejobresponse directsignaturejobresponse)
        {
            return new JobResponse(
                directsignaturejobresponse.signaturejobid,
                new ResponseUrls(
                    new Uri(directsignaturejobresponse.redirecturl),
                    new Uri(directsignaturejobresponse.statusurl)
                    )
                );
        }

        public static DirectJobStatusResponse FromDataTransferObject(directsignaturejobstatusresponse directsignaturejobstatusresponse)
        {
            var jobStatus = directsignaturejobstatusresponse.status.ToJobStatus();

            var jobReferences = jobStatus == JobStatus.Signed
                ? new JobReferences(new Uri(directsignaturejobstatusresponse.confirmationurl), new Uri(directsignaturejobstatusresponse.xadesurl), new Uri(directsignaturejobstatusresponse.padesurl))
                : new JobReferences(null, null, null);

            return new DirectJobStatusResponse(directsignaturejobstatusresponse.signaturejobid, jobStatus, jobReferences);
        }

        internal static directsignaturejobmanifest ToDataTransferObject(Manifest manifest)
        {
            return new directsignaturejobmanifest
            {
                sender = ToDataTransferObject(manifest.Sender),
                document = ToDataTransferObject((Document)manifest.Document),
                signer = ToDataTransferObject(manifest.Signer)
            };
        }

        public static sender ToDataTransferObject(Sender sender)
        {
            return new sender
            {
                organizationnumber = sender.OrganizationNumber
            };
        }

        public static directdocument ToDataTransferObject(Document document)
        {
            return new directdocument
            {
                title = document.Title,
                description = document.Message,
                href = document.FileName,
                mime = document.MimeType
            };
        }

        public static directsigner ToDataTransferObject(AbstractSigner signer)
        {
            return new directsigner
            {
                personalidentificationnumber = signer.PersonalIdentificationNumber.Value
            };
        }
    }
}