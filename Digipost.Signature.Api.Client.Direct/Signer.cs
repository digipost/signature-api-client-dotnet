using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Identifier;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Signer : AbstractSigner
    {
        public SignerIdentifier Identifier { get; }

        public Signer(SignerIdentifier identifier)
        {
            Identifier = identifier;
        }
    }
}