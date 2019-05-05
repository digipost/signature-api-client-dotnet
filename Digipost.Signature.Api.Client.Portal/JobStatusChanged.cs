using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal.Enums;

namespace Digipost.Signature.Api.Client.Portal
{
    public class JobStatusChanged
    {
        [Obsolete("Use NoChangesWithPollTime instead, as this can be removed without further notice.")]
        public static JobStatusChanged NoChanges = new JobStatusChanged(0, null, JobStatus.NoChanges, null, null, DateTime.Now);

        public static JobStatusChanged NoChangesWithPollTime(DateTime nextPermittedPollTime)
        {
            return new JobStatusChanged(0, null, JobStatus.NoChanges, null, null, nextPermittedPollTime);
        }

        [Obsolete("Use constructor with DateTime instead, as this can be removed without further notice.")]
        public JobStatusChanged(long jobId, string jobReference, JobStatus status, ConfirmationReference confirmationReference, List<Signature> signatures)
            : this(jobId, jobReference, status, confirmationReference, signatures, DateTime.Now)
        {
        }

        public JobStatusChanged(long jobId, string jobReference, JobStatus status, ConfirmationReference confirmationReference, List<Signature> signatures, DateTime nextPermittedPollTime)
        {
            JobId = jobId;
            JobReference = jobReference;
            Status = status;
            ConfirmationReference = confirmationReference;
            Signatures = signatures;
            NextPermittedPollTime = nextPermittedPollTime;
        }

        public long JobId { get; internal set; }

        /// <summary>
        ///     Returns the signature job's custom reference as specified upon creation. May be <code>null</code>.
        /// </summary>
        public string JobReference { get; }

        public JobStatus Status { get; set; }

        public ConfirmationReference ConfirmationReference { get; internal set; }

        public PadesReference PadesReference { get; internal set; }

        public List<Signature> Signatures { get; internal set; }

        public DateTime NextPermittedPollTime { get; internal set; }

        public Signature GetSignatureFor(Signer signer)
        {
            return Signatures.Find(elem => signer.Identifier.IsSameAs(elem.Identifier));
        }

        public override string ToString()
        {
            return $"JobId: {JobId}, Status: {Status}, ConfirmationReference: {ConfirmationReference}, PadesReference: {PadesReference}, Signatures: {Signatures}, Next permitted poll time: {NextPermittedPollTime}";
        }
    }
}
