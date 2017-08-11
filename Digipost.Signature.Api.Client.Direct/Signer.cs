using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Signer : AbstractSigner
    {
        public IdentifierType IdentifierType { get; }

        public SignerIdentifier Identifier { get; }

        public Signer(SignerIdentifier identifier)
        {
            Identifier = identifier;
        }
    }

    public enum IdentifierType
    {
        PersonalIdentificationNumber,
        CustomIdentifier
    }
}