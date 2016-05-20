using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectSigner : AbstractSigner
    {
        public DirectSigner(PersonalIdentificationNumber personalIdentificationNumber)
            : base(personalIdentificationNumber)
        {
        }
    }
}
