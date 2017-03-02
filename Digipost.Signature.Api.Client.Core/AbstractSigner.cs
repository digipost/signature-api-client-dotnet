using Digipost.Signature.Api.Client.Core.Enums;

namespace Digipost.Signature.Api.Client.Core
{
    public abstract class AbstractSigner
    {
        protected AbstractSigner(SignerIdentifier signerIdentifier)
        {
            Identifier = signerIdentifier;
        }

        public SignerIdentifier Identifier { get; }

        public int? Order { get; set; } = null;

        public OnBehalfOf OnBehalfOf { get; set; } = OnBehalfOf.Self;

        public SignatureType? SignatureType { get; set; } = null;

        public override string ToString()
        {
            return $"{Identifier.GetType().Name}: {Identifier}, {nameof(Order)}: {Order}, {nameof(SignatureType)}: {SignatureType}";
        }
    }
}