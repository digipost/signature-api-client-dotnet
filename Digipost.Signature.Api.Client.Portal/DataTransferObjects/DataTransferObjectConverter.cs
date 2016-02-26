using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;
using Digipost.Signature.Api.Client.Portal.Enums;
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
                reference = portalJob.Reference
            };

            return portalsignaturejobrequest;
        }

        public static portalsignaturejobmanifest ToDataTransferObject(PortalManifest portalManifest)
        {
            var dataTransferObject = new portalsignaturejobmanifest
            {
                sender = ToDataTransferObject(portalManifest.Sender),
                document = ToDataTransferObject(portalManifest.Document),
                signers = ToDataTransferObject(portalManifest.Signers).ToArray()
            };

            if (portalManifest.Availability != null)
            {
                dataTransferObject.availability = new availability();
                var activationTime = portalManifest.Availability.Activation;
                var expirationTime = portalManifest.Availability.Expiration;

                if (activationTime != null)
                {
                    dataTransferObject.availability.activationtime = portalManifest.Availability.Activation.Value;
                }

                if (expirationTime != null)
                {
                    dataTransferObject.availability.expirationtime = portalManifest.Availability.Expiration.Value;
                }
            }

            return dataTransferObject;
        }

        private static IEnumerable<signer> ToDataTransferObject(IEnumerable<Signer> signers)
        {
            return signers.Select(ToDataTransferObject);
        }

        private static signer ToDataTransferObject(Signer signer)
        {
            return new signer
            {
                personalidentificationnumber = signer.PersonalIdentificationNumber
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

        public static PortalJobResponse FromDataTransferObject(portalsignaturejobresponse portalsignaturejobresponse)
        {
            return new PortalJobResponse(portalsignaturejobresponse.signaturejobid, new Uri(portalsignaturejobresponse.cancellationurl));
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
            var result = new Signature
            {
                SignatureStatus = (SignatureStatus) Enum.Parse(typeof (SignatureStatus), signature.status.ToString(), true),
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