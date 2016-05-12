using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalSigner : Signer
    {
        public PortalSigner(string personalIdentificationNumber)
            : base(personalIdentificationNumber)
        {
        }
    }
}
