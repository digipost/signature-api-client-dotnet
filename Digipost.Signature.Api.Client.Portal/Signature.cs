using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal.Enums;

namespace Digipost.Signature.Api.Client.Portal
{
    public class Signature
    {
        public SignatureStatus SignatureStatus { get; set; }

        public Signer Signer { get; set; }

        public XadesReference XadesReference { get; set; }

    }
}
