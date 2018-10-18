using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Identifier;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Signer : AbstractSigner
    {
        public Signer(SignerIdentifier identifier)
            : base(identifier)
        {
        }
    }
}
