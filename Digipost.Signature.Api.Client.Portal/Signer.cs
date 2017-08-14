using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class Signer : AbstractSigner
    {
        public SignerIdentifier Identifier { get; } 

        public Signer(PersonalIdentificationNumber personalIdentificationNumber, Notifications notifications)
        {
            Identifier = personalIdentificationNumber;
            Notifications = notifications;
        }

        public Signer(PersonalIdentificationNumber personalIdentificationNumber, NotificationsUsingLookup notificationsUsingLookup)
        {
            Identifier = personalIdentificationNumber;
            NotificationsUsingLookup = notificationsUsingLookup;
        }

        public Signer(ContactInformation contactInformation)
        {
            Identifier = contactInformation;
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