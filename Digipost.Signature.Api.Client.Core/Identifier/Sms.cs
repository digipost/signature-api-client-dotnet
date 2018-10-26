namespace Digipost.Signature.Api.Client.Core.Identifier
{
    public class Sms
    {
        public Sms(string number)
        {
            Number = number;
        }

        public string Number { get; }

        public override string ToString()
        {
            return Number;
        }

        public override bool Equals(object other)
        {
            var otherSms = other as Sms;
            return otherSms != null && Number == otherSms.Number;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }
    }
}
