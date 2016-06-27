using System;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct
{
    public class JobStatusResponse
    {

        public static JobStatusResponse NoChanges = new JobStatusResponse(null, JobStatus.NoChanges, null);

        private long? _jobId;

        public JobStatusResponse(long? jobId, JobStatus status, JobReferences references)
        {
            _jobId = jobId;
            Status = status;
            References = references;
        }

        public long JobId
        {
            get
            {
                if (_jobId == null)
                {
                    throw new InvalidOperationException("There were no direct jobs with updated status, and querying the job ID is a programming error. " +
                                                        "Use the Status-property to check if there were any status change before attempting to get any further information.");
                }
                return _jobId.Value;
            }
        }

        public JobStatus Status { get; }

        public JobReferences References { get; }

        public override string ToString()
        {
            return $"JobId: {JobId}, Status: {Status}, References: {References}";
        }
    }
}