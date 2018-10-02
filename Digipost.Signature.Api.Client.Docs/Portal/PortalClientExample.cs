using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Portal;
using Digipost.Signature.Api.Client.Portal.Enums;

namespace Digipost.Signature.Api.Client.Docs.Portal
{
    public class PortalClientExample
    {
        public static async Task CreateAndSend()
        {
            ClientConfiguration clientConfiguration = null; //As initialized earlier
            var portalClient = new PortalClient(clientConfiguration);

            var documentToSign = new Document(
                "Subject of Message",
                "This is the content",
                FileType.Pdf,
                @"C:\Path\ToDocument\File.pdf"
            );

            var signers = new List<Signer>
            {
                new Signer(new PersonalIdentificationNumber("00000000000"), new NotificationsUsingLookup()),
                new Signer(new PersonalIdentificationNumber("11111111111"), new Notifications(
                    new Email("email1@example.com"),
                    new Sms("999999999"))),
                new Signer(new ContactInformation {Email = new Email("email2@example.com")}),
                new Signer(new ContactInformation {Sms = new Sms("88888888")}),
                new Signer(new ContactInformation
                {
                    Email = new Email("email3@example.com"),
                    Sms = new Sms("77777777")
                })
            };

            var portalJob = new Job(documentToSign, signers, "myReferenceToJob");

            var portalJobResponse = await portalClient.Create(portalJob);
        }

        public static async Task GetStatusChange()
        {
            PortalClient portalClient = null; //As initialized earlier

            var jobStatusChanged = await portalClient.GetStatusChange();

            if (jobStatusChanged.Status == JobStatus.NoChanges)
            {
                //Queue is empty. Additional polling will result in blocking for a defined period.
            }
            else
            {
                var signatureJobStatus = jobStatusChanged.Status;
                var signatures = jobStatusChanged.Signatures;
                var signatureOne = signatures.ElementAt(0);
                var signatureOneStatus = signatureOne.SignatureStatus;
            }

            //Polling again:
            try
            {
                var changeResponse2 = await portalClient.GetStatusChange();
            }
            catch (TooEagerPollingException eagerPollingException)
            {
                var nextAvailablePollingTime = eagerPollingException.NextPermittedPollTime;
            }
        }

        public static async Task GetXadesAndPades()
        {
            PortalClient portalClient = null; //As initialized earliers
            var jobStatusChanged = await portalClient.GetStatusChange();

            //Get Xades:
            var xades = await portalClient.GetXades(jobStatusChanged.Signatures.ElementAt(0).XadesReference);

            //Get Pades:
            var pades = await portalClient.GetPades(jobStatusChanged.PadesReference);
        }

        public static async void Confirm()
        {
            PortalClient portalClient = null; //As initialized earlier
            var jobStatusChangeResponse = await portalClient.GetStatusChange();

            await portalClient.Confirm(jobStatusChangeResponse.ConfirmationReference);
        }

        public static async Task SpecifyingQueues()
        {
            PortalClient portalClient = null; //As initialized earlier

            var organizationNumber = "123456789";
            var sender = new Sender(organizationNumber, new PollingQueue("CustomPollingQueue"));

            var documentToSign = new Document(
                "Subject of Message",
                "This is the content",
                FileType.Pdf,
                @"C:\Path\ToDocument\File.pdf"
            );

            var signers = new List<Signer>
            {
                new Signer(new PersonalIdentificationNumber("00000000000"), new NotificationsUsingLookup())
            };

            var portalJob = new Job(documentToSign, signers, "myReferenceToJob", sender);

            var portalJobResponse = await portalClient.Create(portalJob);

            var changedJob = await portalClient.GetStatusChange(sender);
        }
    }
}
