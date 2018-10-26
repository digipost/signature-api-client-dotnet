using System;
using Digipost.Signature.Api.Client.Portal.Enums;

namespace Digipost.Signature.Api.Client.Portal.Extensions
{
    public static class EnumExtensions
    {
        public static JobStatus ToJobStatus(this portalsignaturejobstatus status)
        {
            JobStatus result;
            switch (status)
            {
                case portalsignaturejobstatus.IN_PROGRESS:
                    result = JobStatus.InProgress;
                    break;
                case portalsignaturejobstatus.COMPLETED_SUCCESSFULLY:
                    result = JobStatus.CompletedSuccessfully;
                    break;
                case portalsignaturejobstatus.FAILED:
                    result = JobStatus.Failed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }

            return result;
        }
    }
}
