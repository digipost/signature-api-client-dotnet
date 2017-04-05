using System;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Scripts.XsdToCode.Code;

namespace Digipost.Signature.Api.Client.Direct.Extensions
{
    public static class EnumExtensions
    {
        public static JobStatus ToJobStatus(this directsignaturejobstatus status)
        {
            JobStatus result;
            switch (status)
            {
                case directsignaturejobstatus.COMPLETED_SUCCESSFULLY:
                    result = JobStatus.CompletedSuccessfully;
                    break;
                case directsignaturejobstatus.IN_PROGRESS:
                    result = JobStatus.InProgress;
                    break;
                case directsignaturejobstatus.FAILED:
                    result = JobStatus.Failed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }

            return result;
        }

        public static statusretrievalmethod ToStatusretrievalmethod(this StatusRetrievalMethod status)
        {
            statusretrievalmethod result;
            switch (status)
            {
                case StatusRetrievalMethod.WaitForCallback:
                    result = statusretrievalmethod.WAIT_FOR_CALLBACK;
                    break;
                case StatusRetrievalMethod.Polling:
                    result = statusretrievalmethod.POLLING;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
            return result;
        }
    }
}