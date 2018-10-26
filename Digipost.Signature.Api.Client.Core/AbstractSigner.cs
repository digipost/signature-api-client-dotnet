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
            var order = Order != null ? $"with {nameof(Order).ToLower()} {Order}" : string.Empty;
            var signatureType = SignatureType != null ? $"and {nameof(SignatureType).ToLower()} {SignatureType}" : string.Empty;

            return $"Signer with {nameof(Identifier).ToLower()} '{Identifier}' {order} {signatureType}";
        }
    }
}
