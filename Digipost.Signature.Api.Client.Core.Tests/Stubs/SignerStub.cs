using Digipost.Signature.Api.Client.Core.Identifier;

namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    public class SignerStub : AbstractSigner
    {
        public SignerStub(PersonalIdentificationNumber personalIdentificationNumber)
            :base(personalIdentificationNumber)
        {
        }
    }
}