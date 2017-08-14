namespace Digipost.Signature.Api.Client.Direct
{
    public class CustomIdentifier : Core.SignerIdentifier
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
    }
}