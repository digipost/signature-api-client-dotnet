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

        public override string ToString()
        {
            return $"Sms: {Sms}, Email: {Email}";
        }
    }
}