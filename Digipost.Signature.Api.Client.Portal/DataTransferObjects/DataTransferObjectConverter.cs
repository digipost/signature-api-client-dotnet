using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal.Enums;
using Digipost.Signature.Api.Client.Portal.Extensions;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Portal.DataTransferObjects
{
    internal class DataTransferObjectConverter
    {
        public static portalsignaturejobrequest ToDataTransferObject(Job job)
        {
            var portalsignaturejobrequest = new portalsignaturejobrequest
            {
                reference = job.Reference
            };

            return portalsignaturejobrequest;
        }

        public static portalsignaturejobmanifest ToDataTransferObject(Manifest manifest)
        {
            var dataTransferObject = new portalsignaturejobmanifest
            {
                sender = ToDataTransferObject(manifest.Sender),
                document = ToDataTransferObject(manifest.Document),
                signers = ToDataTransferObject(manifest.Signers).ToArray()
            };

            if (manifest.Availability != null)
            {
                dataTransferObject.availability = new availability();
                var activationTime = manifest.Availability.Activation;

                if (activationTime != null)
                {
                    dataTransferObject.availability.activationtime = activationTime.Value;
                    dataTransferObject.availability.activationtimeSpecified = true;

                }

                var availableSeconds = manifest.Availability.AvailableSeconds;

                if(availableSeconds != null)
                { 
                    dataTransferObject.availability.availableseconds = availableSeconds.Value;
                    dataTransferObject.availability.availablesecondsSpecified = true;
                }
            }

            return dataTransferObject;
        }

        private static IEnumerable<portalsigner> ToDataTransferObject(IEnumerable<Signer> signers)
        {
            return signers.Select(ToDataTransferObject);
        }

        internal static portalsigner ToDataTransferObject(Signer signer)
        {
            var dataTransferObject = new portalsigner
            {
                personalidentificationnumber = signer.PersonalIdentificationNumber.Value
            };

            if (signer.Notifications != null)
            {
                dataTransferObject.Item = ToDataTransferObject(signer.Notifications);
            }
            else
            {
                dataTransferObject.Item = ToDataTransferObject(signer.NotificationsUsingLookup);
            }

            if (signer.Order != null)
            {
                dataTransferObject.order = signer.Order.Value;
            }

            return dataTransferObject;
        }

        internal static notifications ToDataTransferObject(Notifications notifications)
        {
            var notificationsDto = new List<object>();

            if (notifications.Sms!= null)
            {
                notificationsDto.Add(new sms() { number = notifications.Sms.Number});
            }

            if (notifications.Email != null)
            {
                notificationsDto.Add(new email() {address = notifications.Email.Address});
            }

            return new notifications()
            {
                Items = notificationsDto.ToArray()
            };
        }

        internal static notificationsusinglookup ToDataTransferObject(NotificationsUsingLookup notificationsUsingLookup)
        {
            var dataTransferObject = new notificationsusinglookup();

            if (notificationsUsingLookup.EmailIfAvailable)
            {
                dataTransferObject.email = new enabled();
            }

            if (notificationsUsingLookup.SmsIfAvailable)
            {
                dataTransferObject.sms = new enabled();
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

        internal static portaldocument ToDataTransferObject(Document document)
        {
            return new portaldocument
            {
                title = document.Title,
                nonsensitivetitle = document.NonsensitiveTitle,
                description = document.Message,
                href = document.FileName,
                mime = document.MimeType
            };
        }

        public static JobResponse FromDataTransferObject(portalsignaturejobresponse portalsignaturejobresponse)
        {
            return new JobResponse(portalsignaturejobresponse.signaturejobid, new Uri(portalsignaturejobresponse.cancellationurl));
        }

        public static JobStatusChanged FromDataTransferObject(
            portalsignaturejobstatuschangeresponse changeResponse)
        {
            var jobStatus = changeResponse.status.ToJobStatus();
            var confirmationReference = new ConfirmationReference(new Uri(changeResponse.confirmationurl));
            var signatures = FromDataTransferObject(changeResponse.signatures.signature);

            var result = new JobStatusChanged(changeResponse.signaturejobid, jobStatus, confirmationReference, signatures);

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
                SignatureStatus = new SignatureStatus(signature.status),
                Signer = new PersonalIdentificationNumber(signature.personalidentificationnumber)
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