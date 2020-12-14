using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Utilities;
using Digipost.Signature.Api.Client.Portal;
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
            Console.WriteLine("Starting dat loggah ...");
            Log.LogDebug("Debug logging");
            Log.LogInformation("Info logging");
            Log.LogWarning("Warn logging");
            Log.LogError("Error logging");
            Log.LogCritical("Critical logging");

            var stringPrivateOrganizationNumber = "088015814";
            var stringPublicOrganizationNumber = "988015814";

            var testEnvironment = Environment.Test;

            var sender = new Sender(stringPublicOrganizationNumber);
            var pollingQueue = new PollingQueue("dotnet-client-test-simen");
            sender.PollingQueue = pollingQueue;
            
            var client = new PortalClient(
                new ClientConfiguration(
                    testEnvironment,
                    CertificateReader.ReadCertificate(),
                    sender
                ) {LogRequestAndResponse = true}
            );

            var signer = new Signer(new PersonalIdentificationNumber("01043100358"), new Notifications(new Email("test@example.com"))) {OnBehalfOf = OnBehalfOf.Other};

            var created = client.Create(new Job(
                new Document("Et signeringsoppdrag", "Her kommer en melding", FileType.Pdf,  AppDomain.CurrentDomain.BaseDirectory + "/../../../handverkerkontrakt.pdf"),
                new List<Signer> {signer},
                "Avsenders referance", sender)).Result;

            Log.LogInformation("Opprettet portaloppdrag: " + created.JobId);

            while (true)
            {
                var status = client.GetStatusChange(sender).Result;
                
                Log.LogInformation("Hentet status for: " + status.JobId + " - " + status.Status);
                Log.LogInformation("Minimum tid til neste poll: " + status.NextPermittedPollTime);
                Task.Delay((TimeSpan.FromSeconds(5))).Wait();
            }
        }
    }
    
    
}
