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
