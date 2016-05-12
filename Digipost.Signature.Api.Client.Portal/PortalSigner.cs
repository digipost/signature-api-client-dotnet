using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalSigner : Signer
    {
        public NotificationsUsingLookup NotificationsUsingLookup { get; }

        public Notifications Notifications { get; }

        public PortalSigner(string personalIdentificationNumber, Notifications notifications)
            : base(personalIdentificationNumber)
        {
            Notifications = notifications;
        }

        public PortalSigner(string personalIdentificationNumber, NotificationsUsingLookup notificationsUsingLookup)
    : base(personalIdentificationNumber)
        {
            NotificationsUsingLookup = notificationsUsingLookup;
        }
    }
}
