using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJobStatusResponse
    {
        public long JobId { get; }

        public JobStatus JobStatus { get; }

        public StatusResponseUrls StatusResponseUrls { get; }

        public DirectJobStatusResponse(long jobId, JobStatus jobStatus, StatusResponseUrls statusResponseUrls)
        {
            JobId = jobId;
            JobStatus = jobStatus;
            StatusResponseUrls = statusResponseUrls;
        }
    }
}
