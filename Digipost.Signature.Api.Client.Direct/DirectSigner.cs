using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectSigner : Signer
    {
        public DirectSigner(PersonalIdentificationNumber personalIdentificationNumber)
            : base(personalIdentificationNumber)
        {
        }
    }
}
