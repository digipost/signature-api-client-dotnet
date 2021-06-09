using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Utilities;
using Digipost.Signature.Api.Client.Direct;
using Digipost.Signature.Api.Client.Portal;
using Digipost.Signature.Api.Client.Portal.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.Program
{
    public class Program
    {
        private static readonly ILogger<Program> Log = LoggingUtility.CreateServiceProviderAndSetUpLogging().GetService<ILoggerFactory>().CreateLogger<Program>();

        private static void Main(string[] args)
        {
            var stringPrivateOrganizationNumber = "088015814";
            var stringPublicOrganizationNumber = "988015814";

            var testEnvironment = Environment.Test;

            var sender = new Sender(stringPublicOrganizationNumber);

            var clientConfiguration = new ClientConfiguration(testEnvironment,
                CertificateReader.ReadCertificate(),
                sender);
            var documentsToSign = new List<Document>
            {
                new Document(
                    "Kontrakt",
                    FileType.Pdf,
                    AppDomain.CurrentDomain.BaseDirectory + "/../../../handverkerkontrakt.pdf"),
                new Document(
                    "Vedlegg",
                    FileType.Pdf,
                    AppDomain.CurrentDomain.BaseDirectory + "/../../../PDF_A-inputdokument.pdf"),
                new Document(
                    "Vedlegg 2",
                    FileType.Pdf,
                    AppDomain.CurrentDomain.BaseDirectory + "/../../../PDF_A_testdokument.pdf"),
            };

            // PortalFlow(clientConfiguration, documentsToSign, stringPublicOrganizationNumber).Wait();
            DirectFlow(clientConfiguration, documentsToSign);
        }

        private static void DirectFlow(ClientConfiguration clientConfiguration, List<Document> documentsToSign)
        {
            var directClient = new DirectClient(clientConfiguration);

            var exitUrls = new ExitUrls(new Uri("https://example.com"),
                new Uri("https://example.com"),
                new Uri("https://example.com"));

            var signers = new List<Direct.Signer>()
            {
                new Direct.Signer(new PersonalIdentificationNumber("27099044395"))
            };

            var job = new Direct.Job("JobTitle", documentsToSign, signers, "*****", exitUrls);

            var directJobResponse = directClient.Create(job).Result;
            foreach (var signer in directJobResponse.Signers)
            {
                Log.LogInformation(signer.RedirectUrl.AbsoluteUri);
            }
        }
        
        private static async Task PortalFlow(ClientConfiguration clientConfiguration, List<Document> documentsToSign, string stringPrivateOrganizationNumber)
        {
            var client = new PortalClient(clientConfiguration);
            var sender = new Sender(stringPrivateOrganizationNumber)
            {
                PollingQueue = new PollingQueue("tester-lokalt")
            };
            var signer = new Portal.Signer(new PersonalIdentificationNumber("01043100358"), new Notifications(new Email("email@example.com"))) {OnBehalfOf = OnBehalfOf.Other};

            var jobToCreate = new Portal.Job(
                "Job title",
                documentsToSign,
                new List<Portal.Signer> {signer},
                "Avsenders referance", sender
            )
            {
                Description =  "Beskrivelse av jobben",
                NonSensitiveTitle = "Tittel som vises i mail"
            };
            var created = client.Create(jobToCreate).Result;

            Log.LogInformation($"Created portal job: {created.JobId}");

            var waiting = true;
            while (waiting)
            {
                try
                {
                    var status = client.GetStatusChange(sender).Result;
                    Log.LogInformation($"Job: {status.JobId}, Status: {status.Status}");
                    
                    if (status.Status == JobStatus.CompletedSuccessfully) 
                    {
                        var file = File.Create(Path.Combine("..", "..", "..", "pades_from_portal_flow.pdf"));
                        using (var padesByteStream = await client.GetPades(status.PadesReference))
                        using (var fileStream = new FileStream(file.SafeFileHandle, FileAccess.ReadWrite))
                        {
                            await padesByteStream.CopyToAsync(fileStream);
                        }
                        Log.LogInformation($"Downloaded PAdES: {file.Name}");
                    }
                    
                    if (status.Status != JobStatus.NoChanges)
                    {
                        await client.Confirm(status.ConfirmationReference);
                    }

                    if (created.JobId == status.JobId)
                    {
                        if (status.Status == JobStatus.CompletedSuccessfully || status.Status == JobStatus.Failed)
                        {
                            waiting = false;
                        }
                    }
                    await Task.Delay(5000);
                }
                catch (Exception e)
                {
                    Log.LogError("Exception: ", e.Message);
                }
            }
        }
    }
    
    
}
