namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalJobResponse
    {
        public long JobId { get; internal set; }

        public PortalJobResponse(long jobId)
        {
            JobId = jobId;
        }
    }
}