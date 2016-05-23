namespace Digipost.Signature.Api.Client.Core
{
    public class PersonalIdentificationNumber
    {
        public PersonalIdentificationNumber(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        private string MaskedValue => Value.Substring(0, 6) + "*****";

        public override string ToString()
        {
            return $"Value: {MaskedValue}";
        }
    }
}