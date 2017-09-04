namespace Digipost.Signature.Api.Client.Core.Identifier
{
    public class PersonalIdentificationNumber : SignerIdentifier
    {
        public string Value { get; }

        public PersonalIdentificationNumber(string value)
        {
            Value = value;
        }

        private string MaskedValue => Value.Substring(0, 6) + "*****";

        public override string ToString()
        {
            return $"personal identification number of {MaskedValue}";
        }

        public override bool IsSameAs(SignerIdentifier other)
        {
            if (other is PersonalIdentificationNumber otherPersonalIdentificationNumber)
            {
                return IsEqual(Value, otherPersonalIdentificationNumber.Value);
            }

            return false;
        }
    }
}