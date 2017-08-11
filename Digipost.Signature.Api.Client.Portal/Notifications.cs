namespace Digipost.Signature.Api.Client.Portal
{
    public class Notifications
    {
        public Notifications(Email email, Sms sms = null)
        {
            Email = email;
            Sms = sms;
        }

        public Notifications(Sms sms, Email email = null)
        {
            Email = email;
            Sms = sms;
        }

        public Sms Sms { get; internal set; }

        public Email Email { get; internal set; }

        public bool ShouldSendSms => Sms != null;

        public bool ShouldSendEmail => Email != null;

        public override string ToString()
        {
            if (ShouldSendEmail && ShouldSendSms)
            {
                return "Notifications to " + Email + " and " + Sms;
            }

            if (ShouldSendEmail)
            {
                return "Notification to " + Email;
            }

            if (ShouldSendSms)
            {
                return "Notification to " + Sms;
            }

            return "No notifications";
        }
    }
}