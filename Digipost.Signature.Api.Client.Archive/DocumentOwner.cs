namespace Digipost.Signature.Api.Client.Archive
{
    public class DocumentOwner
    {
        public DocumentOwner(string organizationNumber)
        {
            OrganizationNumber = organizationNumber;
        }

        public string OrganizationNumber { get; }
    }
}
