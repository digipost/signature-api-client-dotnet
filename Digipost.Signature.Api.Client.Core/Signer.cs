namespace Digipost.Signature.Api.Client.Core
{
    public class Signer
    {
        public Signer(string personalIdentificationNumber)
        {
            PersonalIdentificationNumber = personalIdentificationNumber;
        }

        public string PersonalIdentificationNumber { get; }
    }
}