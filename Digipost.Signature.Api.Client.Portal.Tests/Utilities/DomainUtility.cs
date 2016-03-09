using System;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;

namespace Digipost.Signature.Api.Client.Portal.Tests.Utilities
{
    internal class DomainUtility
    {
        internal static SignatureGenerator GetSignature()
        {
            return new SignatureGenerator(CoreDomainUtility.GetTestCertificate(), CoreDomainUtility.GetDocument(), GetPortalManifest());
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
            return new PortalJob(CoreDomainUtility.GetDocument(), CoreDomainUtility.GetSigners(signers), "PortalJobReference");
        }

        internal static PortalManifest GetPortalManifest()
        {
            return new PortalManifest(
                CoreDomainUtility.GetSender(),
                CoreDomainUtility.GetDocument(),
                CoreDomainUtility.GetSigners(2)
                );
        }
    }
}