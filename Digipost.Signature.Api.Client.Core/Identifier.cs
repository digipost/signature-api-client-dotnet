namespace Digipost.Signature.Api.Client.Core
{
    public class Identifier
    {
        public Identifier(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Identifier;

            return other != null && string.Equals(other.Value, Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return 17 * 23 + Value.GetHashCode();
            }
        }
    }
}