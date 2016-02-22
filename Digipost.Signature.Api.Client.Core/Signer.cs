namespace Digipost.Signature.Api.Client.Core
{
    public class Signer
    {
        public string PersonalIdentificationNumber { get; }

        public Signer(string personalIdentificationNumber)
        {
            PersonalIdentificationNumber = personalIdentificationNumber;
        }

    }
}