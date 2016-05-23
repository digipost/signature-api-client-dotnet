using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJobStatusResponse
    {
        public DirectJobStatusResponse(long jobId, JobStatus status, JobReferences references)
        {
            JobId = jobId;
            Status = status;
            References = references;
        }

        public long JobId { get; }

        public JobStatus Status { get; }

        public JobReferences References { get; }

        public override string ToString()
        {
            return $"JobId: {JobId}, Status: {Status}, References: {References}";
        }
    }
}