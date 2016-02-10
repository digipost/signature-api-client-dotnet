using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal.Extensions;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Portal.DataTransferObjects
{
    internal class DataTransferObjectConverter
    {
        public static portalsignaturejobrequest ToDataTransferObject(PortalJob portalJob)
        {
            var portalsignaturejobrequest = new portalsignaturejobrequest
            {
                reference = portalJob.Reference,
            };

            return portalsignaturejobrequest;
        }

        public static portalsignaturejobmanifest ToDataTransferObject(PortalManifest portalManifest)
        {
            return new portalsignaturejobmanifest()
            {
                sender = ToDataTransferObject(portalManifest.Sender),
                document = ToDataTransferObject(portalManifest.Document)
            };
        }

        public static sender ToDataTransferObject(Sender sender)
        {
            return new sender()
            {
                organizationnumber = sender.OrganizationNumber
            };
        }

        public static document ToDataTransferObject(Document document)
        {
            return new document()
            {
                title = document.Subject,
                description = document.Message,
                href = document.FileName,
                mime = document.MimeType
            };
        }

        public static PortalJobResponse FromDataTransferObject(portalsignaturejobresponse dtoPortalsignaturejobresponse)
        {
            return new PortalJobResponse(dtoPortalsignaturejobresponse.signaturejobid);
        }

        public static PortalJobStatusChangeResponse FromDataTransferObject(
            portalsignaturejobstatuschangeresponse changeResponse)
        {
            var jobStatus = changeResponse.status.ToJobStatus();
            var confirmationReference = new ConfirmationReference(new Uri(changeResponse.confirmationurl));
            var signatures = FromDataTransferObject(changeResponse.signatures.signature);

            var result = new PortalJobStatusChangeResponse(changeResponse.signaturejobid, jobStatus, confirmationReference, signatures);

            var padesUrl = changeResponse.signatures.padesurl;
            if (padesUrl != null)
            {
                result.PadesReference = new PadesReference(new Uri(padesUrl));
            }

            return result;
        }

        private static List<Signature> FromDataTransferObject(signature[] signatures)
        {
            return signatures.Select(FromDataTransferObject).ToList();
        }

        private static Signature FromDataTransferObject(signature signature)
        {
            var result = new Signature()
            {
                SignatureStatus = (SignatureStatus) Enum.Parse(typeof(SignatureStatus), signature.status.ToString(),ignoreCase:true),
                Signer = new Signer(signature.personalidentificationnumber)
            };

            var xadesUrl = signature.xadesurl;
            if (!string.IsNullOrEmpty(xadesUrl))
            {
                result.XadesReference = new XadesReference(new Uri(xadesUrl));
            }

            return result;
        }
    }
}
