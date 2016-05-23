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
    }
}