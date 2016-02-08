using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal.DataTransferObjects
{
    internal class DataTransferObjectConverter
    {
        public static portalsignaturejobrequest ToDataTransferObject(PortalJob portalJob, Sender sender)
        {
            var portalsignaturejobrequest = new portalsignaturejobrequest
            {
                sender = new sender { organization = sender.OrganizationNumber},
                reference = portalJob.Reference,
                signers = ToDataTransferObject(portalJob.Signers).ToArray()
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
    }
}
