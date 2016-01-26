using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJobStatusResponse
    {
        public long JobId { get; private set; }

        public JobStatus JobStatus { get; private set; }

        public StatusResponseUrls StatusResponseUrls { get; private set; }

        public DirectJobStatusResponse(long jobId, JobStatus jobStatus, StatusResponseUrls statusResponseUrls)
        {
            JobId = jobId;
            JobStatus = jobStatus;
            StatusResponseUrls = statusResponseUrls;
        }
    }
}
