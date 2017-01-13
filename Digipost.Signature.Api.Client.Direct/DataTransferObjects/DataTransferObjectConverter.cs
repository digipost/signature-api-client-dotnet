using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Extensions;
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
                exiturls = ToDataTransferObject(job.ExitUrls),
                statusretrievalmethod = job.StatusRetrievalMethod.ToStatusretrievalmethod(),
                statusretrievalmethodSpecified = true
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
                    directsignaturejobresponse.redirecturl.Select(redirecturl => new RedirectReference(new Uri(redirecturl.Value), new PersonalIdentificationNumber(redirecturl.signer))).ToList(),
                    directsignaturejobresponse.statusurl == null ? null : new Uri(directsignaturejobresponse.statusurl)
                )
            );
        }

        public static JobStatusResponse FromDataTransferObject(directsignaturejobstatusresponse directsignaturejobstatusresponse)
        {
            var jobStatus = directsignaturejobstatusresponse.signaturejobstatus.ToJobStatus();

            var signatures = new List<Signature>();
            foreach (var signerstatus in directsignaturejobstatusresponse.status)
            {
                var xadesurl = directsignaturejobstatusresponse.xadesurl?.SingleOrDefault(xades => xades.signer.Equals(signerstatus.signer));
                var xadesReference = xadesurl == null ? null : new XadesReference(new Uri(xadesurl.Value));
                var signature = new Signature(new PersonalIdentificationNumber(signerstatus.signer), xadesReference, new SignatureStatus(signerstatus.Value));
                signatures.Add(signature);
            }

            var jobReferences = new JobReferences(
                directsignaturejobstatusresponse.confirmationurl == null ? null : new Uri(directsignaturejobstatusresponse.confirmationurl),
                directsignaturejobstatusresponse.padesurl == null ? null : new Uri(directsignaturejobstatusresponse.padesurl)
            );

            return new JobStatusResponse(directsignaturejobstatusresponse.signaturejobid, jobStatus, jobReferences, signatures);
        }

        internal static directsignaturejobmanifest ToDataTransferObject(Manifest manifest)
        {
            var dataTransferObject = new directsignaturejobmanifest
            {
                sender = ToDataTransferObject(manifest.Sender),
                document = ToDataTransferObject((Document) manifest.Document),
                signer = ToDataTransferObject(manifest.Signer).ToArray()
            };

            if (manifest.AuthenticationLevel != null)
            {
                dataTransferObject.requiredauthentication = manifest.AuthenticationLevel.Value.ToAuthenticationlevel();
                dataTransferObject.requiredauthenticationSpecified = true;
            }

            return dataTransferObject;
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

        private static IEnumerable<directsigner> ToDataTransferObject(IEnumerable<AbstractSigner> signers)
        {
            return signers.Select(ToDataTransferObject);
        }

        public static directsigner ToDataTransferObject(AbstractSigner signer)
        {
            var dataTransferObject = new directsigner
            {
                personalidentificationnumber = signer.PersonalIdentificationNumber.Value
            };

            if (signer.SignatureType != null)
            {
                dataTransferObject.signaturetype = signer.SignatureType.Value.ToSignaturtype();
                dataTransferObject.signaturetypeSpecified = true;
            }

            return dataTransferObject;
        }
    }
}