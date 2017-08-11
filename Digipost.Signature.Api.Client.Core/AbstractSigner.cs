using Digipost.Signature.Api.Client.Core.Enums;

namespace Digipost.Signature.Api.Client.Core
{
    public abstract class AbstractSigner
    {
        public int? Order { get; set; } = null;

        public OnBehalfOf OnBehalfOf { get; set; } = OnBehalfOf.Self;

        public SignatureType? SignatureType { get; set; } = null;

        public override string ToString()
        {
            return $"{nameof(Order)}: {Order}, {nameof(SignatureType)}: {SignatureType}";
        }
    }
}