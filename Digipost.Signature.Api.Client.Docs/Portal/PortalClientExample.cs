using System;
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

            var documentsToSign = new List<Document>
            {
                new Document(
                    "Document title",
                    FileType.Pdf,
                    @"C:\Path\ToDocument\File.pdf"
                )
            };

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

            var portalJob = new Job("Job title", documentsToSign, signers, "myReferenceToJob");

            var portalJobResponse = await portalClient.Create(portalJob);
        }

        public static async Task GetStatusChange()
        {
            PortalClient portalClient = null; //As initialized earlier

            // Repeat the polling until signer signs the document, but ensure to do this at a 
            // reasonable interval. If you are processing the result a few times a day in your
            // system, only poll a few times a day.
            var change = await portalClient.GetStatusChange();

            switch (change.Status)
            {
                case JobStatus.NoChanges:
                    //Queue is empty. Additional polling will result in blocking for a defined period.
                    break;
                case JobStatus.Failed:
                case JobStatus.InProgress:
                case JobStatus.CompletedSuccessfully:
                {
                    var signatureJobStatus = change.Status;
                    var signatures = change.Signatures;
                    var signatureOne = signatures.ElementAt(0);
                    var signatureOneStatus = signatureOne.SignatureStatus;
                    break;
                }
            }

            var pollingWillResultInBlock = change.NextPermittedPollTime > DateTime.Now;
            if (pollingWillResultInBlock)
            {
                //Wait until next permitted poll time has passed before polling again.
            }

            //Confirm the receipt to remove it from the queue
            await portalClient.Confirm(change.ConfirmationReference);
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

        public static async void Cancel()
        {
            PortalClient portalClient = null; //As initialized earlier
            Job portalJob = null; //As initialized earlier

            var portalJobResponse = await portalClient.Create(portalJob);

            await portalClient.Cancel(portalJobResponse.CancellationReference);
        }

        public static async Task SpecifyingQueues()
        {
            PortalClient portalClient = null; //As initialized earlier

            var organizationNumber = "123456789";
            var sender = new Sender(organizationNumber, new PollingQueue("CustomPollingQueue"));

            var documentsToSign = new List<Document>
            {
                new Document(
                    "Document title",
                    FileType.Pdf,
                    @"C:\Path\ToDocument\File.pdf"
                )
            };

            var signers = new List<Signer>
            {
                new Signer(new PersonalIdentificationNumber("00000000000"), new NotificationsUsingLookup())
            };

            var portalJob = new Job("Job title", documentsToSign, signers, "myReferenceToJob", sender);

            var portalJobResponse = await portalClient.Create(portalJob);

            var changedJob = await portalClient.GetStatusChange(sender);
        }
    }
}
