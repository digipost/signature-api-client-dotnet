using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Signature
    {
        public Signature(PersonalIdentificationNumber signer, XadesReference xadesReference, SignatureStatus signatureStatus)
        {
            Signer = signer;
            XadesReference = xadesReference;
            SignatureStatus = signatureStatus;
        }

        public PersonalIdentificationNumber Signer { get; set; }

        public XadesReference XadesReference { get; set; }

        public SignatureStatus SignatureStatus { get; set; }
    }
}