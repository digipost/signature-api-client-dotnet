using System;

namespace Digipost.Signature.Api.Client.Portal.Extensions
{
    public static class EnumExtensions
    {
        public static JobStatus ToJobStatus(this portalsignaturejobstatus status)
        {
            JobStatus result;
            switch (status)
            {
                case portalsignaturejobstatus.PARTIALLY_COMPLETED:
                    result = JobStatus.PartiallyCompleted;
                    break;
                case portalsignaturejobstatus.COMPLETED:
                    result = JobStatus.Completed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }

            return result;
        }
    }
}
