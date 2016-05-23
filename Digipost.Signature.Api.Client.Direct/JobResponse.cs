namespace Digipost.Signature.Api.Client.Direct
{
    public class JobResponse
    {
        public JobResponse(long jobId, ResponseUrls responseUrls)
        {
            JobId = jobId;
            ResponseUrls = responseUrls;
        }

        public long JobId { get; }

        public ResponseUrls ResponseUrls { get; }

        public override string ToString()
        {
            return $"JobId: {JobId}, ResponseUrls: {ResponseUrls}";
        }
    }
}