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
