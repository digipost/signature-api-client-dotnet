namespace Digipost.Signature.Api.Client.Portal
{
    public class Sms
    {
        public Sms(string number)
        {
            Number = number;
        }

        public string Number { get; set; }

        public override string ToString()
        {
            return $"Number: {Number}";
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