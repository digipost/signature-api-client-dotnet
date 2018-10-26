namespace Digipost.Signature.Api.Client.Core
{
    public class Sender
    {
        public Sender(string organizationNumber)
            : this(organizationNumber, PollingQueue.Default)
        {
        }

        public Sender(string organizationNumber, PollingQueue pollingQueue)
        {
            OrganizationNumber = organizationNumber;
            PollingQueue = pollingQueue;
        }

        public string OrganizationNumber { get; set; }

        public PollingQueue PollingQueue { get; set; }
    }
}
