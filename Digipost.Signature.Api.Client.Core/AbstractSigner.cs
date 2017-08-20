using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Identifier;

namespace Digipost.Signature.Api.Client.Core
{
    public abstract class AbstractSigner
    {
        protected AbstractSigner(SignerIdentifier identifier)
        {
            Identifier = identifier;
        }

        public SignerIdentifier Identifier { get; }

        public int? Order { get; set; } = null;

        public OnBehalfOf OnBehalfOf { get; set; } = OnBehalfOf.Self;

        public SignatureType? SignatureType { get; set; } = null;

        public override string ToString()
        {
            return $"{nameof(Order)}: {Order}, {nameof(SignatureType)}: {SignatureType}";
        }
    }
}