using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectSigner : Signer
    {
        public DirectSigner(string personalIdentificationNumber)
            : base(personalIdentificationNumber)
        {
        }
    }
}
