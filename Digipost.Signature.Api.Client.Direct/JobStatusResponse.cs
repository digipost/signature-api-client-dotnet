using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct
{
    public class JobStatusResponse
    {
        public static JobStatusResponse NoChanges(DateTime nextPermittedPollTime)
        {
            return new JobStatusResponse(null, null, JobStatus.NoChanges, null, null, nextPermittedPollTime);
        } 

        private long? _jobId;

        public JobStatusResponse(long? jobId, string jobReference, JobStatus status, JobReferences references, List<Signature> signatures, DateTime nextPermittedPollTime )
        {
            _jobId = jobId;
            JobReference = jobReference;
            Status = status;
            References = references;
            Signatures = signatures;
            NextPermittedPollTime = nextPermittedPollTime;
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

        /// <summary>
        ///     Returns the signature job's custom reference as specified upon creation. May be <code>null</code>.
        /// </summary>
        public string JobReference { get; }

        public JobStatus Status { get; }

        public JobReferences References { get; }

        public List<Signature> Signatures { get; internal set; }
        
        public DateTime NextPermittedPollTime { get; }


        /// <summary>
        ///     Gets the signature from a given signer.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the job response doesn't contain a signature for this signer</exception>
        /// <seealso cref="Signatures" />
        public Signature GetSignatureFor(SignerIdentifier signer)
        {
            Signature foundSignature = null;
            if (signer is PersonalIdentificationNumber)
            {
                foundSignature = Signatures.SingleOrDefault(s => s.Signer == ((PersonalIdentificationNumber) signer).Value);
            }

            if (signer is CustomIdentifier)
            {
                foundSignature = Signatures.SingleOrDefault(s => s.Signer == ((CustomIdentifier) signer).Value);
            }

            if (foundSignature == null)
            {
                throw new InvalidOperationException($"Unable to find signature for Signer '{signer}'");
            }

            return foundSignature;
        }

        public override string ToString()
        {
            return $"JobId: {JobId}, Status: {Status}, References: {References}, Next permitted poll time: {NextPermittedPollTime}";
        }
    }
}
