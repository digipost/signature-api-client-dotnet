using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJobStatusResponse
    {
        public long JobId { get; private set; }

        public JobStatus Status { get; private set; }

        public JobReferences References { get; private set; }

        public DirectJobStatusResponse(long jobId, JobStatus status, JobReferences references)
        {
            JobId = jobId;
            Status = status;
            References = references;
        }
    }
}
