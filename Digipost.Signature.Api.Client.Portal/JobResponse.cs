using System;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class JobResponse
    {
        public JobResponse(long jobId, string jobReference, Uri cancellationUrl)
        {
            JobId = jobId;
            JobReference = jobReference;
            CancellationReference = new CancellationReference(cancellationUrl);
        }

        public long JobId { get; }

        /// <summary>
        ///     Returns the signature job's custom reference as specified upon creation. May be <code>null</code>.
        /// </summary>
        public string JobReference { get; }

        public CancellationReference CancellationReference { get; set; }

        public override string ToString()
        {
            return $"JobId: {JobId}, CancellationReference: {CancellationReference}";
        }
    }
}
