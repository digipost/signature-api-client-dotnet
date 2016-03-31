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

        public static SignatureStatus ToSignatureStatus(this signaturestatus status)
        {
            SignatureStatus result;

            switch (status)
            {
                case signaturestatus.WAITING:
                    result = SignatureStatus.Waiting;
                    break;
                case signaturestatus.REJECTED:
                    result = SignatureStatus.Rejected;
                    break;
                case signaturestatus.CANCELLED:
                    result = SignatureStatus.Cancelled;
                    break;
                case signaturestatus.EXPIRED:
                    result = SignatureStatus.Expired;
                    break;
                case signaturestatus.SIGNED:
                    result = SignatureStatus.Signed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }

            return result;
        }
    }
}