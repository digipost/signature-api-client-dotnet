using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Extensions;
using Digipost.Signature.Api.Client.Core.Identifier;
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

            var senderPollingQueue = job.Sender.PollingQueue.Name;
            if (!string.IsNullOrEmpty(senderPollingQueue))
            {
                portalsignaturejobrequest.pollingqueue = senderPollingQueue;
            }

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

            if (manifest.AuthenticationLevel != null)
            {
                dataTransferObject.requiredauthentication = manifest.AuthenticationLevel.Value.ToAuthenticationlevel();
                dataTransferObject.requiredauthenticationSpecified = true;
            }

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

                if (availableSeconds != null)
                {
                    dataTransferObject.availability.availableseconds = availableSeconds.Value;
                    dataTransferObject.availability.availablesecondsSpecified = true;
                }
            }

            if (manifest.IdentifierInSignedDocuments != null)
            {
                dataTransferObject.identifierinsigneddocuments = manifest.IdentifierInSignedDocuments.Value.ToIdentifierInSignedDocuments();
                dataTransferObject.identifierinsigneddocumentsSpecified = true;
            }

            return dataTransferObject;
        }

        private static IEnumerable<portalsigner> ToDataTransferObject(IEnumerable<Signer> signers)
        {
            return signers.Select(ToDataTransferObject);
        }

        internal static portalsigner ToDataTransferObject(Signer signer)
        {
            var dataTransferObject = new portalsigner();
            var notifications = signer.Notifications;

            if (signer.Identifier is PersonalIdentificationNumber)
            {
                dataTransferObject.Item = ((PersonalIdentificationNumber) signer.Identifier).Value;

                if (notifications != null)
                {
                    dataTransferObject.Item1 = ToDataTransferObject(notifications);
                }
                else
                {
                    dataTransferObject.Item1 = ToDataTransferObject(signer.NotificationsUsingLookup);
                }
            }
            else
            {
                if (notifications.ShouldSendEmail || notifications.ShouldSendSms)
                {
                    dataTransferObject.Item = new enabled();
                    dataTransferObject.Item1 = ToDataTransferObject(notifications);
                }
            }

            dataTransferObject.onbehalfof = signer.OnBehalfOf.ToSigningonbehalfof();
            dataTransferObject.onbehalfofSpecified = true;

            if (signer.Order != null)
            {
                dataTransferObject.order = signer.Order.Value;
            }

            if (signer.SignatureType != null)
            {
                dataTransferObject.signaturetype = signer.SignatureType.Value.ToSignaturtype();
                dataTransferObject.signaturetypeSpecified = true;
            }

            return dataTransferObject;
        }

        internal static notifications ToDataTransferObject(Notifications notifications)
        {
            var notificationsDto = new List<object>();

            if (notifications.ShouldSendEmail)
            {
                notificationsDto.Add(new email {address = notifications.Email.Address});
            }

            if (notifications.ShouldSendSms)
            {
                notificationsDto.Add(new sms {number = notifications.Sms.Number});
            }

            return new notifications
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

        public static JobStatusChanged FromDataTransferObject(portalsignaturejobstatuschangeresponse changeResponse)
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

        private static List<Signature> FromDataTransferObject(IEnumerable<signature> signatures)
        {
            return signatures.Select(FromDataTransferObject).ToList();
        }

        private static Signature FromDataTransferObject(signature signature)
        {
            return new Signature(signature);
        }
    }
}