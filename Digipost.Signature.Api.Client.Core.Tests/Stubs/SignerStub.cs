namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    public class SignerStub : AbstractSigner
    {
        public string Identifier { get; set; }

        public SignerStub(PersonalIdentificationNumber personalIdentificationNumber)
        {
            Identifier = personalIdentificationNumber.Value;
        }
    }
}