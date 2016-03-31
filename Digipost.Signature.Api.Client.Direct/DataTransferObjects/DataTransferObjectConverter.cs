using System;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Extensions;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Direct.DataTransferObjects
{
    public static class DataTransferObjectConverter

    {
        public static directsignaturejobrequest ToDataTransferObject(DirectJob directJob)
        {
            return new directsignaturejobrequest
            {
                reference = directJob.Reference,
                exiturls = ToDataTransferObject(directJob.ExitUrls)
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

        public static DirectJobResponse FromDataTransferObject(directsignaturejobresponse directsignaturejobresponse)
        {
            return new DirectJobResponse(
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

        internal static directsignaturejobmanifest ToDataTransferObject(DirectManifest directManifest)
        {
            return new directsignaturejobmanifest
            {
                sender = ToDataTransferObject(directManifest.Sender),
                document = ToDataTransferObject(directManifest.Document),
                signer = ToDataTransferObject(directManifest.Signer)
            };
        }

        public static sender ToDataTransferObject(Sender sender)
        {
            return new sender
            {
                organizationnumber = sender.OrganizationNumber
            };
        }

        public static document ToDataTransferObject(Document document)
        {
            return new document
            {
                title = document.Subject,
                description = document.Message,
                href = document.FileName,
                mime = document.MimeType
            };
        }

        public static signer ToDataTransferObject(Signer signer)
        {
            return new signer
            {
                personalidentificationnumber = signer.PersonalIdentificationNumber
            };
        }
    }
}