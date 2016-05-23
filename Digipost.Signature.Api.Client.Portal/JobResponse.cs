using System;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class JobResponse
    {
        public JobResponse(long jobId, Uri cancellationUrl)
        {
            JobId = jobId;
            CancellationReference = new CancellationReference(cancellationUrl);
        }

        public long JobId { get; }

        public CancellationReference CancellationReference { get; set; }

        public override string ToString()
        {
            return $"JobId: {JobId}, CancellationReference: {CancellationReference}";
        }
    }
}