namespace Digipost.Signature.Api.Client.Core
{
    public abstract class Signer
    {
         protected Signer(string personalIdentificationNumber)
        {
            PersonalIdentificationNumber = personalIdentificationNumber;
        }

        public string PersonalIdentificationNumber { get; }

        public int? Order { get; set; } = null;

        private string MaskedPersonalIdentificationNumber => PersonalIdentificationNumber.Substring(0, 6) + "*****";

        public override string ToString()
        {
            return $"PersonalIdentificationNumber: {MaskedPersonalIdentificationNumber}, Order: {Order}";
        }
    }
}