using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal.Enums;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalJobStatusChangeResponse
    {
        public long JobId { get; internal set; }

        public JobStatus Status { get; set; }

        public ConfirmationReference ConfirmationReference { get; internal set; }

        public PadesReference PadesReference { get; internal set; }

        public List<Signature> Signatures { get; internal set; }

        public PortalJobStatusChangeResponse(long jobId, JobStatus status, ConfirmationReference confirmationReference, List<Signature> signatures)
        {
            JobId = jobId;
            Status = status;
            ConfirmationReference = confirmationReference;
            Signatures = signatures;
        }
    }
}