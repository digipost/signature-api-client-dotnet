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

        public static PortalJob GetPortalJob(int signers)
        {
            return new PortalJob(GetPortalDocument(), GetSigners(signers), "PortalJobReference")
            {
                Availability = new Availability
                {
                    Activation = DateTime.Now,
                    AvailableFor = new TimeSpan(0, 0, 10, 0)
                }
            };
        }

        internal static PortalManifest GetPortalManifest()
        {
            return new PortalManifest(
                CoreDomainUtility.GetSender(),
                GetPortalDocument(),
                GetSigners(2)
                );
        }

        internal static PortalDocument GetPortalDocument()
        {
            return new PortalDocument("TheTitle", "Some cool portal document message", "TheFileName", FileType.Pdf, CoreDomainUtility.GetPdfDocumentBytes())
            {
                NonsensitiveTitle = "The nonsensitve title"
            };
        }

        public static Signer GetSigner()
        {
            return GetSigners(1).First();
        }

        public static List<PortalSigner> GetSigners(int count)
        {
            if (count > 9)
            {
                throw new ArgumentException("Maximum of 9 senders.");
            }

            var signers = new List<PortalSigner>();

            const string basePersonalIdentificationNumber = "0101330000";
            for (var i = 1; i <= count; i++)
            {
                signers.Add(new PortalSigner(new PersonalIdentificationNumber(basePersonalIdentificationNumber + i), new NotificationsUsingLookup()));
            }

            return signers;
        }
    }
}