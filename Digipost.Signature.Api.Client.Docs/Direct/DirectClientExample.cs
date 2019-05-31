using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Direct;
using Digipost.Signature.Api.Client.Direct.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Document = Digipost.Signature.Api.Client.Direct.Document;

namespace Digipost.Signature.Api.Client.Docs.Direct
{
    public class DirectClientExample
    {
        private ILogger<DirectClientExample> _logger = new NullLogger<DirectClientExample>();

        public async Task CreateAndSendSignatureJob()
        {
            ClientConfiguration clientConfiguration = null; //As initialized earlier
            var directClient = new DirectClient(clientConfiguration);

            var documentToSign = new Document(
                "Subject of Message",
                "This is the content",
                FileType.Pdf,
                @"C:\Path\ToDocument\File.pdf");

            var exitUrls = new ExitUrls(
                new Uri("http://redirectUrl.no/onCompletion"),
                new Uri("http://redirectUrl.no/onCancellation"),
                new Uri("http://redirectUrl.no/onError")
            );

            var signers = new List<Signer>
            {
                new Signer(new PersonalIdentificationNumber("12345678910")),
                new Signer(new PersonalIdentificationNumber("10987654321"))
            };

            var job = new Job(documentToSign, signers, "SendersReferenceToSignatureJob", exitUrls);

            var directJobResponse = await directClient.Create(job);
        }

        public async Task RequestNewRedirectUrlFromResponse()
        {
            ClientConfiguration clientConfiguration = null; //As initialized earlier
            Job job = null; //As created earlier
            var directClient = new DirectClient(clientConfiguration);
            var directJobResponse = await directClient.Create(job);

            var signerFromResponse = directJobResponse
                .Signers
                .First(s => s
                    .Identifier
                    .IsSameAs(new PersonalIdentificationNumber("12345678910"))
                );

            var newRedirectUrl = await directClient.RequestNewRedirectUrl(signerFromResponse);
        }

        public async Task RequestNewRedirectUrlFromSignerUrl()
        {
            ClientConfiguration clientConfiguration = null; //As initialized earlier
            Job job = null; //As created earlier
            var directClient = new DirectClient(clientConfiguration);
            var directJobResponse = await directClient.Create(job);

            // Step 1:
            foreach (var signerResponse in directJobResponse.Signers)
            {
                //Persist signer URL in sender system
                var signerResponseSignerUrl = signerResponse.SignerUrl;
            }

            // ... some time later ...

            // Step 2: Request new redirect URL for signer
            Uri persistedSignerUrl = null; //Persisted URL from step 1.
            var signerWithUpdatedRedirectUrl = await directClient
                .RequestNewRedirectUrl(
                    NewRedirectUrlRequest
                        .FromSignerUrl(persistedSignerUrl)
                );

            var newRedirectUrl2 = await directClient.RequestNewRedirectUrl(signerWithUpdatedRedirectUrl);
        }

        public async Task GetDirectJobStatus()
        {
            ClientConfiguration clientConfiguration = null; //As initialized earlier
            var directClient = new DirectClient(clientConfiguration);
            JobResponse jobResponse = null; //As initialized when creating signature job
            var statusQueryToken = "0A3BQ54C...";

            var jobStatusResponse =
                await directClient.GetStatus(jobResponse.StatusUrl.Status(statusQueryToken));

            var jobStatus = jobStatusResponse.Status;
        }

        public async Task PollForStatus()
        {
            ClientConfiguration clientConfiguration = null; // As initialized earlier
            var directClient = new DirectClient(clientConfiguration);

            // Repeat the polling until signer signs the document, but ensure to do this at a 
            // reasonable interval. If you are processing the result a few times a day in your
            // system, only poll a few times a day.
            var change = await directClient.GetStatusChange();

            switch (change.Status)
            {
                case JobStatus.NoChanges:
                    // Queue is empty. Additional polling will result in blocking for a defined period.
                    break;
                case JobStatus.CompletedSuccessfully:
                    // Get PAdES
                    // Get XAdES
                    break;
                case JobStatus.Failed:
                    break;
                case JobStatus.InProgress:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Confirm status change to avoid receiving it again
            await directClient.Confirm(change.References.Confirmation);

            var pollingWillResultInBlock = change.NextPermittedPollTime > DateTime.Now;
            if (pollingWillResultInBlock)
            {
                //Wait until next permitted poll time has passed before polling again.
            }
        }

        public async Task GetXadesAndPades()
        {
            ClientConfiguration clientConfiguration = null; //As initialized earlier
            var directClient = new DirectClient(clientConfiguration);
            JobStatusResponse jobStatusResponse = null; // Result of requesting job status

            if (jobStatusResponse.Status == JobStatus.CompletedSuccessfully)
            {
                var padesByteStream = await directClient.GetPades(jobStatusResponse.References.Pades);
            }

            var signature = jobStatusResponse.GetSignatureFor(new PersonalIdentificationNumber("00000000000"));

            if (signature.Equals(SignatureStatus.Signed))
            {
                var xadesByteStream = await directClient.GetXades(signature.XadesReference);
            }
        }

        public async Task ConfirmSignatureJob()
        {
            ClientConfiguration clientConfiguration = null; //As initialized earlier
            var directClient = new DirectClient(clientConfiguration);
            JobStatusResponse jobStatusResponse = null; // Result of requesting job status

            await directClient.Confirm(jobStatusResponse.References.Confirmation);
        }

        public async Task SpecifyingQueues()
        {
            ClientConfiguration clientConfiguration = null; // As initialized earlier
            var directClient = new DirectClient(clientConfiguration);

            var organizationNumber = "123456789";
            var sender = new Sender(organizationNumber, new PollingQueue("CustomPollingQueue"));

            Document documentToSign = null; // As initialized earlier
            ExitUrls exitUrls = null; // As initialized earlier

            var signer = new PersonalIdentificationNumber("00000000000");

            var job = new Job(
                documentToSign,
                new List<Signer> {new Signer(signer)},
                "SendersReferenceToSignatureJob",
                exitUrls,
                sender,
                StatusRetrievalMethod.Polling
            );

            await directClient.Create(job);

            var changedJob = await directClient.GetStatusChange(sender);
        }

        public async Task ErrorHandling()
        {
            /**
            There are different forms of exceptions that can occur. Some are more specific than others.
            All exceptions related to client behavior inherits from `SignatureException`. 
            **/

            try
            {
                //Some signature action
                await SomeSignatureAction();
            }
            catch (BrokerNotAuthorizedException notAuthorizedException)
            {
                _logger.LogError(notAuthorizedException, "Not authorized to perform action. The correct access rights for organization are not set.");
            }
            catch (UnexpectedResponseException unexpectedResponseException)
            {
                //UnexpectedResponseException will normally contain an `Error` object giving a more detailed error description. If this error does not exist, 
                // you can still get the status code and message.
                var statusCode = unexpectedResponseException.StatusCode;
                var responseMessage = unexpectedResponseException.Message;

                if (unexpectedResponseException.Error != null)
                {
                    var errorMessage = unexpectedResponseException.Error.Message;
                    var errorType = unexpectedResponseException.Error.Type;
                }
            }
            catch (SignatureException exception)
            {
                _logger.LogError(exception, "Something went wrong");
            }
        }

#pragma warning disable 1998
        private static async Task SomeSignatureAction()
#pragma warning restore 1998
        {
        }
    }
}
