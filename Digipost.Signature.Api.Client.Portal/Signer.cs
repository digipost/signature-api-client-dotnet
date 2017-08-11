using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class Signer : AbstractSigner
    {
        public string Identifier { get; } //Todo: Gjor om til type: signeridentifier

        public IdentifierType IdentifierType { get; internal set; }

        public Signer(PersonalIdentificationNumber personalIdentificationNumber, Notifications notifications)
        {
            IdentifierType = IdentifierType.PersonalIdentificationNumber;
            Identifier = personalIdentificationNumber.Value;
            Notifications = notifications;
        }

        public Signer(PersonalIdentificationNumber personalIdentificationNumber, NotificationsUsingLookup notificationsUsingLookup)
        {
            IdentifierType = IdentifierType.PersonalIdentificationNumber;
            Identifier = personalIdentificationNumber.Value;
            NotificationsUsingLookup = notificationsUsingLookup;
        }

        public Signer(ContactInformation contactInformation)
        {
            IdentifierType = contactInformation.Type;
            Notifications = new Notifications(contactInformation.Email, contactInformation.Sms);
        }

        public NotificationsUsingLookup NotificationsUsingLookup { get; }

        public Notifications Notifications { get; }

        public override string ToString()
        {
            return $"{base.ToString()}, NotificationsUsingLookup: {NotificationsUsingLookup}, Notifications: {Notifications}";
        }
    }

    public enum IdentifierType
    {
        PersonalIdentificationNumber,
        Email,
        Sms,
        EmailAndSms //Todo: Ikke helt komfortabel med at det heter Sms. Hvor mye brekker vi om vi gjor om til mobile number?
    }
}