using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Logging;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Portal;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.TestClient
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            Console.WriteLine("Starting dat loggah ...");
            Log.Debug("Debug logging");
            Log.Info("Info logging");
            Log.Warn("Warn logging");
            Log.Error("Error logging");
            Log.Fatal("Fatal logging");

            var stringPrivateOrganizationNumber = "088015814";
            var stringPublicOrganizationNumber = "988015814";

            var testEnvironment = Environment.DifiTest;
            testEnvironment.Url = new Uri("https://api.test.signering.posten.no");

            var client = new PortalClient(
                new ClientConfiguration(
                    testEnvironment,
                    "2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed",
                    new Sender(stringPublicOrganizationNumber)
                ) {LogRequestAndResponse = true}
            );

            var signer = new Signer(new PersonalIdentificationNumber("01043100358"), new Notifications(new Email("email@example.com"))) { OnBehalfOf = OnBehalfOf.Other };

            var created = client.Create(new Job(
                new Document("Et signeringsoppdrag", "Her kommer en melding", FileType.Pdf, @"\\vmware-host\Shared Folders\Downloads\00370726201232902222.pdf"),
                new List<Signer>() {signer},
                "Avsenders referance", new Sender(stringPrivateOrganizationNumber))).Result;

            var status = client.GetStatusChange(new Sender(stringPrivateOrganizationNumber)).Result;
            


            Console.WriteLine("Finished with loggah ...");
            Console.ReadLine();
        }
    }
}