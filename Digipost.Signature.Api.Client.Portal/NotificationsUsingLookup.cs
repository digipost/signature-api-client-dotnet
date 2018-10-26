namespace Digipost.Signature.Api.Client.Portal
{
    public class NotificationsUsingLookup
    {
        public bool SmsIfAvailable { get; set; }

        public bool EmailIfAvailable { get; } = true;

        public override string ToString()
        {
            return $"SmsIfAvailable: {SmsIfAvailable}, EmailIfAvailable: {EmailIfAvailable}";
        }
    }
}
