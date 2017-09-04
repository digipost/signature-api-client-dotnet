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
            return other is Sms otherSms && Number == otherSms.Number;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }
    }
}