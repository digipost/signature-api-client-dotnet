using System;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalJobResponse
    {
        public PortalJobResponse(long jobId, Uri cancellationUrl)
        {
            JobId = jobId;
            CancellationReference = new CancellationReference(cancellationUrl);
        }

        public long JobId { get; }

        public CancellationReference CancellationReference { get; set; }
    }
}