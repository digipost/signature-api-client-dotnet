using System;
using System.Collections.Generic;
using System.Linq;
using Schemas;

namespace Digipost.Signature.Api.Client.Direct
{
    public class JobResponse
    {
        public JobResponse(long jobId, string jobReference, IEnumerable<SignerResponse> signers, StatusUrl statusUrl)
        {
            JobId = jobId;
            JobReference = jobReference;
            Signers = signers.ToList();
            StatusUrl = statusUrl;
        }

        internal JobResponse(directsignaturejobresponse jobResponse)
            : this(
                jobResponse.signaturejobid,
                jobResponse.reference,
                jobResponse.signer.Select(signer => new SignerResponse(signer)),
                jobResponse.statusurl != null ? new StatusUrl(new Uri(jobResponse.statusurl)) : null
            )
        {
        }

        public long JobId { get; }

        /// <summary>
        ///     Returns the signature job's custom reference as specified upon creation. May be <code>null</code>.
        /// </summary>
        public string JobReference { get; }

        public StatusUrl StatusUrl { get; }

        public IEnumerable<SignerResponse> Signers { get; }

        public override bool Equals(object obj)
        {
            return obj is JobResponse that
                   && JobId.Equals(that.JobId)
                   && JobReference.Equals(that.JobReference)
                   && Signers.Equals(that.Signers)
                   && StatusUrl.Equals(that.StatusUrl);
        }

        public override int GetHashCode()
        {
            return JobId.GetHashCode()
                   + JobReference.GetHashCode()
                   + Signers.GetHashCode()
                   + StatusUrl.GetHashCode();
        }

        public override string ToString()
        {
            var signers = string.Join(",", Signers);
            return $"A job with id '{JobId}', reference '{JobReference}', signers '{signers}," +
                   $" and {StatusUrl} ";
        }
    }
}
