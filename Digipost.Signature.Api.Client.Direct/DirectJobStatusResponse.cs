using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJobStatusResponse
    {
        public long JobId { get; private set; }

        public JobStatus JobStatus { get; private set; }

        public JobReferences JobReferences { get; private set; }

        public DirectJobStatusResponse(long jobId, JobStatus jobStatus, JobReferences jobReferences)
        {
            JobId = jobId;
            JobStatus = jobStatus;
            JobReferences = jobReferences;
        }
    }
}
