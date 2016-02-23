namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalJobResponse
    {
        public PortalJobResponse(long jobId)
        {
            JobId = jobId;
        }

        public long JobId { get; internal set; }
    }
}