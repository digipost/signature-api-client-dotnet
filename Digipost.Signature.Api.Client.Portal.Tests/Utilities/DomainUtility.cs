using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Portal.Tests.Utilities
{
    internal class DomainUtility
    {
        internal static SignatureGenerator GetSignature()
        {
            return new SignatureGenerator(CoreDomainUtility.GetTestCertificate(), GetPortalDocument(), GetPortalManifest());
        }

        public static Availability GetAvailability()
        {
            return new Availability
            {
                Activation = DateTime.Now.AddHours(2),
                AvailableFor = new TimeSpan(1, 0, 0, 0)
            };
        }

        public static Job GetPortalJob()
        {
            return new Job(GetPortalDocument(), GetSigner(), "PortalJobReference")
            {
                Availability = new Availability
                {
                    Activation = DateTime.Now,
                    AvailableFor = new TimeSpan(0, 0, 10, 0)
                }
            };
        }

        internal static Manifest GetPortalManifest()
        {
            return new Manifest(
                CoreDomainUtility.GetSender(),
                GetPortalDocument(),
                GetSigners(2)
                );
        }

        internal static Document GetPortalDocument()
        {
            return new Document("TheTitle", "Some cool portal document message", "TheFileName", FileType.Pdf, CoreDomainUtility.GetPdfDocumentBytes());
        }

        public static List<Signer> GetSigner()
        {
            return new List<Signer>()
            {
                new Signer(new PersonalIdentificationNumber("01043100358"), new NotificationsUsingLookup())
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
                signers.Add(new Signer(new PersonalIdentificationNumber(basePersonalIdentificationNumber + i), new NotificationsUsingLookup()));
            }

            return signers;
        }
    }
}