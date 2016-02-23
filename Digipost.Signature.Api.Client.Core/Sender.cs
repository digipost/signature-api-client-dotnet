namespace Digipost.Signature.Api.Client.Core
{
    public class Sender
    {
        public Sender(string organizationNumber)
        {
            OrganizationNumber = organizationNumber;
        }

        public string OrganizationNumber { get; }
    }
}