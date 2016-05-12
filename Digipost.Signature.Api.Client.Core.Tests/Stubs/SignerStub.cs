namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    public class SignerStub : Signer
    {
        public SignerStub(string personalIdentificationNumber)
            : base(personalIdentificationNumber)
        {
        }
    }
}
