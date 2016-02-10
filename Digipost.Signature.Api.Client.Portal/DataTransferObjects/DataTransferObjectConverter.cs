using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal.Extensions;

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

        private static IEnumerable<signer> ToDataTransferObject(IEnumerable<Signer> signers)
        {
            return signers.Select(signer => ToDataTransferObject(signer)).ToList();
        }

        private static signer ToDataTransferObject(Signer signer)
        {
            return new signer
            {
                personalidentificationnumber = signer.PersonalIdentificationNumber
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

        private static Signature FromDataTransferObject(signature signatures)
        {
            var result = new Signature()
            {
                SignatureStatus = (SignatureStatus) Enum.Parse(typeof(SignatureStatus), signatures.status.ToString(),ignoreCase:true),
                Signer = new Signer(signatures.personalidentificationnumber)
            };

            var xadesUrl = signatures.xadesurl;
            if (!string.IsNullOrEmpty(xadesUrl))
            {
                result.XadesReference = new XadesReference(new Uri(xadesUrl));
            }

            return result;
        }
    }
}
