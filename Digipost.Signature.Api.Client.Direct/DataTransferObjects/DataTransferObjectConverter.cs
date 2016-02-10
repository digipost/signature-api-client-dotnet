using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct.DataTransferObjects
{
    public static class DataTransferObjectConverter

    {
        public static directsignaturejobrequest ToDataTransferObject(DirectJob directJob, Sender sender)
        {
            return new directsignaturejobrequest()
            {
                reference = directJob.Reference,
                sender = ToDataTransferObject(sender),
                signer = ToDataTransferObject(directJob.Signer),
                exiturls = ToDataTransferObject(directJob.ExitUrls)
            };
        }

        public static exiturls ToDataTransferObject(ExitUrls exitUrls)
        {
            return new exiturls
            {
                cancellationurl = exitUrls.CancellationUrl.AbsoluteUri,
                completionurl = exitUrls.CompletionUrl.AbsoluteUri,
                errorurl = exitUrls.ErrorUrl.AbsoluteUri
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
            var jobStatus = (JobStatus)Enum.Parse(typeof(JobStatus), directsignaturejobstatusresponse.status.ToString(), ignoreCase: true);

            JobReferences jobReferences;
            
            if (jobStatus == JobStatus.Signed)
            {
                jobReferences = new JobReferences(
                    confirmation: new Uri(directsignaturejobstatusresponse.confirmationurl),
                    xades: new Uri(directsignaturejobstatusresponse.xadesurl),
                    pades: new Uri(directsignaturejobstatusresponse.padesurl)
                );
            }
            else
            {
                jobReferences = new JobReferences(confirmation: null, xades: null, pades: null);
            }

            return new DirectJobStatusResponse(directsignaturejobstatusresponse.signaturejobid, jobStatus, jobReferences);
        }

        public static directsignaturejobmanifest ToDataTransferObject(Manifest manifest)
        {
            return new directsignaturejobmanifest();
            {
                SenderDataTransferObject = ToDataTransferObject(manifest.Sender),
                DocumentDataTransferObject = ToDataTransferObject(manifest.Document),
                SignersDataTransferObjects = ToDataTransferObject(manifest.Signers).ToList()
            };
        }

        public static SenderDataTransferObject ToDataTransferObject(Sender sender)
        {
            return new SenderDataTransferObject()
            {
                Organization = sender.OrganizationNumber
            };
        }

        public static DocumentDataTransferObject ToDataTransferObject(Document document)
        {
            return new DocumentDataTransferObject()
            {
                Title = document.Subject,
                Description = document.Message,
                Href = document.FileName,
                Mime = document.MimeType
            };
        }

        public static IEnumerable<SignerDataTranferObject> ToDataTransferObject(IEnumerable<Signer> signers)
        {
            return signers.Select(signer => ToDataTransferObject(signer)).ToList();
        }

        public static SignerDataTranferObject ToDataTransferObject(Signer signer)
        {
            return new SignerDataTranferObject()
            {
                PersonalIdentificationNumber = signer.PersonalIdentificationNumber
            };
        }

    }
}
