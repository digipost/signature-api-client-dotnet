namespace Digipost.Signature.Api.Client.Core
{
    public class PersonalIdentificationNumber : Identifier
    {
        public PersonalIdentificationNumber(string value) : base(value)
        {
        }

        private string MaskedValue => Value.Substring(0, 6) + "*****";

        public override string ToString()
        {
            return $"{nameof(MaskedValue)}: {MaskedValue}";
        }
    }
}