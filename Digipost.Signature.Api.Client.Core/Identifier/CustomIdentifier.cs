namespace Digipost.Signature.Api.Client.Core.Identifier
{
    public class CustomIdentifier : SignerIdentifier
    {
        public CustomIdentifier(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return $"custom identifier of {Value}";
        }

        public override bool Equals(object obj)
        {
            var other = obj as CustomIdentifier;

            return other != null &&
                   other.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool IsSameAs(SignerIdentifier other)
        {
            var otherCustomIdentifier = other as CustomIdentifier;

            return otherCustomIdentifier != null
                   && IsEqual(Value, otherCustomIdentifier.Value);
        }
    }
}