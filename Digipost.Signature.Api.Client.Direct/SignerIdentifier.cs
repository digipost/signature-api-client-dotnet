using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class SignerIdentifier : Identifier
    {
        public SignerIdentifier(string identifier) : base(identifier)
        {
        }

        public override string ToString()
        {
            return $"{nameof(Value)}: {Value}";
        }
    }
}
