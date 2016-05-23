using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class Signature
    {
        public SignatureStatus SignatureStatus { get; set; }

        public PersonalIdentificationNumber Signer { get; set; }

        public XadesReference XadesReference { get; set; }

        public override string ToString()
        {
            return $"SignatureStatus: {SignatureStatus}, Signer: {Signer}, XadesReference: {XadesReference}";
        }
    }
}