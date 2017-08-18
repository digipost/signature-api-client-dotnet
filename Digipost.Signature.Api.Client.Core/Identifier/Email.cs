namespace Digipost.Signature.Api.Client.Portal
{
    public class Email
    {
        public Email(string address)
        {
            Address = address;
        }

        public string Address { get; set; }
    }
}