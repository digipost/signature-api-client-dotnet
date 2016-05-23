namespace Digipost.Signature.Api.Client.Core
{
    public abstract class AbstractSigner
    {
        protected AbstractSigner(PersonalIdentificationNumber personalIdentificationNumber)
        {
            PersonalIdentificationNumber = personalIdentificationNumber;
        }

        public PersonalIdentificationNumber PersonalIdentificationNumber { get; }

        public int? Order { get; set; } = null;

        public override string ToString()
        {
            return $"PersonalIdentificationNumber: {PersonalIdentificationNumber}, Order: {Order}";
        }
    }
}