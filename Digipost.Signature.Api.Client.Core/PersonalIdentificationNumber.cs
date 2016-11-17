namespace Digipost.Signature.Api.Client.Core
{
    public class PersonalIdentificationNumber
    {
        public PersonalIdentificationNumber(string value)
        {
            Value = value;
        }

        public string Value { get; }

        private string MaskedValue => Value.Substring(0, 6) + "*****";

        public override string ToString()
        {
            return $"{MaskedValue}";
        }

        public override bool Equals(object obj)
        {
            var other = obj as PersonalIdentificationNumber;

            return (other != null) && string.Equals(other.Value, Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash*23 + Value.GetHashCode();
                return hash;
            }
        }
    }
}