using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal.Enums;

namespace Digipost.Signature.Api.Client.Portal
{
    public class JobStatusChanged
    {
        public static JobStatusChanged NoChangesJobStatusChanged = new JobStatusChanged(0, JobStatus.NoChanges, null, null);

        public JobStatusChanged(long jobId, JobStatus status, ConfirmationReference confirmationReference, List<Signature> signatures)
        {
            JobId = jobId;
            Status = status;
            ConfirmationReference = confirmationReference;
            Signatures = signatures;
        }

        public long JobId { get; internal set; }

        public JobStatus Status { get; set; }

        public ConfirmationReference ConfirmationReference { get; internal set; }

        public PadesReference PadesReference { get; internal set; }

        public List<Signature> Signatures { get; internal set; }

        public Signature GetSignatureFor(Signer signer)
        {
            return Signatures.Find(elem => signer.Identifier.IsSameAs(elem.Identifier));
        }

        public override string ToString()
        {
            return $"JobId: {JobId}, Status: {Status}, ConfirmationReference: {ConfirmationReference}, PadesReference: {PadesReference}, Signatures: {Signatures}";
        }
    }
}