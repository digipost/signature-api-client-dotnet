using System;
using System.Linq;
using Digipost.Signature.Api.Client.Core.Identifier;
using Schemas;

namespace Digipost.Signature.Api.Client.Direct
{
    public class JobResponse
    {
        public JobResponse(long jobId, string jobReference, ResponseUrls responseUrls)
        {
            JobId = jobId;
            JobReference = jobReference;
            ResponseUrls = responseUrls;
        }

        internal JobResponse(directsignaturejobresponse jobResponse)
        {
            JobId = jobResponse.signaturejobid;
            JobReference = jobResponse.reference;
            ResponseUrls = new ResponseUrls(
                jobResponse.redirecturl
                    .Select(redirectUrl =>
                        new RedirectReference(
                            new Uri(redirectUrl.Value),
                            new PersonalIdentificationNumber(redirectUrl.signer)))
                    .ToList(),
                jobResponse.statusurl == null ? null : new Uri(jobResponse.statusurl)
            );
        }

        public long JobId { get; }

        /// <summary>
        ///     Returns the signature job's custom reference as specified upon creation. May be <code>null</code>.
        /// </summary>
        public string JobReference { get; }

        public ResponseUrls ResponseUrls { get; }

        public override string ToString()
        {
            return $"JobId: {JobId}, ResponseUrls: {ResponseUrls}";
        }
    }
}
