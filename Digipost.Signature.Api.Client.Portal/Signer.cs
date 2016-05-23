using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class Signer : AbstractSigner
    {
        public Signer(PersonalIdentificationNumber personalIdentificationNumber, Notifications notifications)
            : base(personalIdentificationNumber)
        {
            Notifications = notifications;
        }

        public Signer(PersonalIdentificationNumber personalIdentificationNumber, NotificationsUsingLookup notificationsUsingLookup)
            : base(personalIdentificationNumber)
        {
            NotificationsUsingLookup = notificationsUsingLookup;
        }

        public NotificationsUsingLookup NotificationsUsingLookup { get; }

        public Notifications Notifications { get; }
    }
}