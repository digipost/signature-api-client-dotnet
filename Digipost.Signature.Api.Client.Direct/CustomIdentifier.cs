using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class CustomIdentifier : SignerIdentifier
    {
        public CustomIdentifier(string identifier) : base(identifier)
        {
        }

        public override string ToString()
        {
            return $"custom identifier of {Value}";
        }
    }
}
