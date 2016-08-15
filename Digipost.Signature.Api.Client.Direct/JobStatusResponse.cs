using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct
{
    public class JobStatusResponse
    {
        public static JobStatusResponse NoChanges = new JobStatusResponse(null, JobStatus.NoChanges, null, null);

        private long? _jobId;

        public JobStatusResponse(long? jobId, JobStatus status, JobReferences references, List<Signature> signatures)
        {
            _jobId = jobId;
            Status = status;
            References = references;
            Signatures = signatures;
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

        public List<Signature> Signatures { get; internal set; }

        /// <summary>
        ///     Gets the signature from a given signer.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the job response doesn't contain a signature from this signer</exception>
        /// <seealso cref="Signatures" />
        public Signature GetSignatureFrom(PersonalIdentificationNumber signer)
        {
            var signature = Signatures.SingleOrDefault(s => s.Signer.Equals(signer));
            if (signature == null)
            {
                throw new InvalidOperationException($"Unable to find signature from {signer}");
            }
            return signature;
        }

        public override string ToString()
        {
            return $"JobId: {JobId}, Status: {Status}, References: {References}";
        }
    }
}