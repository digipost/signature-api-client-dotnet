using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Signer : AbstractSigner
    {
        public Signer(SignerIdentifier signerIdentifier)
            : base(signerIdentifier)
        {
        }
    }
}