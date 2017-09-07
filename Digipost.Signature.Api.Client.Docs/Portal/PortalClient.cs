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
    public class Class1
    {
        public static void CreateClientConfiguration()
        {
            const string organizationNumber = "123456789";
            const string certificateThumbprint = "3k 7f 30 dd 05 d3 b7 fc...";

            var clientConfiguration = new ClientConfiguration(
                Environment.DifiTest,
                certificateThumbprint,
                new Sender(organizationNumber));
        }

        public static async Task CreateAndSend()
        {
            PortalClient portalClient = null; //As initialized earlier

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
                new Signer(new ContactInformation {Email = new Email("email3@example.com"), Sms = new Sms("77777777")})
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
    }
}