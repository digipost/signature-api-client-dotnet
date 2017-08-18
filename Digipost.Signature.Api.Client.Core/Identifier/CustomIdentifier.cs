namespace Digipost.Signature.Api.Client.Core.Identifier
{
    public class CustomIdentifier : SignerIdentifier
    {
        public string Value { get; }

        public CustomIdentifier(string value)
        {
            Value = value;
        }

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
    }
}