using Digipost.Signature.Api.Client.Portal;

namespace Digipost.Signature.Api.Client.Core.Identifier
{
    public abstract class SignerIdentifier
    {
        public bool IsContactInformation()
        {
            return this is ContactInformation;
        }

        public bool IsPersonalIdentificationNumber()
        {
            return this is PersonalIdentificationNumber;
        }

        public bool IsCustomIdentifier()
        {
            return this is CustomIdentifier;
        }

        public ContactInformation ToContactInformation()
        {
            return (ContactInformation) this;
        }

        public PersonalIdentificationNumber ToPersonalIdentificationNumber()
        {
            return (PersonalIdentificationNumber) this;
        }

        public CustomIdentifier ToCustomIdentifier()
        {
            return (CustomIdentifier) this;
        }
    }
}
