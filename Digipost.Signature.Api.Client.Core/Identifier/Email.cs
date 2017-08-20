namespace Digipost.Signature.Api.Client.Portal
{
    public class Email
    {
        public Email(string address)
        {
            Address = address;
        }

        public string Address { get; }

        public override bool Equals(object other)
        {
            return other is Email otherEmail && Address == otherEmail.Address;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }
    }
}