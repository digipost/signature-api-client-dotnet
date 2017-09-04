namespace Digipost.Signature.Api.Client.Core.Identifier
{
    public class PersonalIdentificationNumber : SignerIdentifier
    {
        public PersonalIdentificationNumber(string value)
        {
            Value = value;
        }

        public string Value { get; }

        private string MaskedValue => Value.Substring(0, 6) + "*****";

        public override string ToString()
        {
            return $"personal identification number of {MaskedValue}";
        }

        public override bool IsSameAs(SignerIdentifier other)
        {
            if (other is PersonalIdentificationNumber)
            {
                var otherPersonalIdentificationNumber = (PersonalIdentificationNumber) other;
                return IsEqual(Value, otherPersonalIdentificationNumber.Value);
            }

            return false;
        }
    }
}