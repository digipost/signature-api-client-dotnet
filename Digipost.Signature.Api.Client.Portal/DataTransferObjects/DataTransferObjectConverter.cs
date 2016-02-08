using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal.DataTransferObjects
{
    internal class DataTransferObjectConverter
    {
        public static DTOPortalsignaturejobrequest ToDataTransferObject(PortalJob portalJob, Sender sender)
        {
            var portalsignaturejobrequest = new DTOPortalsignaturejobrequest
            {
                Sender = new DTOSender { Organization = sender.OrganizationNumber},
                Reference = portalJob.Reference,
                Signers = ToDataTransferObject(portalJob.Signers).ToList()
            };

            return portalsignaturejobrequest;
        }

        private static IEnumerable<DTOSigner> ToDataTransferObject(IEnumerable<Signer> signers)
        {
            return signers.Select(signer => ToDataTransferObject(signer)).ToList();
        }

        private static DTOSigner ToDataTransferObject(Signer signer)
        {
            return new DTOSigner
            {
                Personalidentificationnumber = signer.PersonalIdentificationNumber
            };
        }

        public static PortalJobResponse FromDataTransferObject(DTOPortalsignaturejobresponse dtoPortalsignaturejobresponse)
        {
            return new PortalJobResponse(dtoPortalsignaturejobresponse.Signaturejobid);
        }
    }
}
