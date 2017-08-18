using Digipost.Signature.Api.Client.Core.Identifier;

namespace Digipost.Signature.Api.Client.Core
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
    }
}