namespace Digipost.Signature.Api.Client.Core
{
    public class Sender
    {
        public string OrganizationNumber { get; }

        public Sender(string organizationNumber)
        {
            OrganizationNumber = organizationNumber;
        }
    }
}
