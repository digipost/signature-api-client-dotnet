using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Internal.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Portal.Tests.Utilities
{
    internal class DomainUtility
    {
        internal static SignatureGenerator GetSignature()
        {
            return new SignatureGenerator(CoreDomainUtility.GetTestCertificate(), GetPortalDocuments(), GetPortalManifest());
        }

        public static Availability GetAvailability()
        {
            return new Availability
            {
                Activation = DateTime.Now.AddHours(2),
                AvailableFor = new TimeSpan(1, 0, 0, 0)
            };
        }

        public static Job GetPortalJob(params Signer[] signers)
        {
            return new Job(
                "JobTitle",
                GetPortalDocuments(),
                signers.Length == 0 ? GetSignerWithPersonalIdentificationNumber() : signers.ToList(),
                "PortalJobReference"
            );
        }

        internal static Manifest GetPortalManifest()
        {
            return new Manifest(
                "JobTitle",
                CoreDomainUtility.GetSender(),
                GetPortalDocuments(),
                GetSigners(2)
            );
        }

        internal static Document GetPortalDocument()
        {
            return new Document("TheTitle", FileType.Pdf, CoreDomainUtility.GetPdfDocumentBytes());
        }
        
        internal static List<Document> GetPortalDocuments()
        {
            return new List<Document>{ GetPortalDocument() };
        }

        public static List<Signer> GetSignerWithPersonalIdentificationNumber()
        {
            return new List<Signer>
            {
                new Signer(new PersonalIdentificationNumber("01043100358"), new Notifications(new Email("email@example.com")))
            };
        }

        public static List<Signer> GetSigners(int count)
        {
            if (count > 9)
            {
                throw new ArgumentException("Maximum of 9 senders.");
            }

            var signers = new List<Signer>();

            const string basePersonalIdentificationNumber = "0101330000";
            for (var i = 1; i <= count; i++)
            {
                signers.Add(new Signer(new PersonalIdentificationNumber(basePersonalIdentificationNumber + i), new Notifications(new Email("email@example.com"))));
            }

            return signers;
        }
    }
}
