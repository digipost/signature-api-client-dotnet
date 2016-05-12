namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    public class SignerStub : Signer
    {
        public SignerStub(PersonalIdentificationNumber personalIdentificationNumber)
            : base(personalIdentificationNumber)
        {
        }
    }
}
