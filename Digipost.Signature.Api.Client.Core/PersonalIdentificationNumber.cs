namespace Digipost.Signature.Api.Client.Core
{
    public class PersonalIdentificationNumber
    {
        public string Value { get; set; }

        public PersonalIdentificationNumber(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"Value: {MaskedValue}";
        }

        private string MaskedValue => Value.Substring(0, 6) + "*****";

    }
}