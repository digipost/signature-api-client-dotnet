namespace Digipost.Signature.Api.Client.Direct
{
    public class JobResponse
    {
        public JobResponse(long jobId, ResponseUrls responseUrls)
        {
            JobId = jobId;
            ResponseUrls = responseUrls;
        }

        public long JobId { get; private set; }

        public ResponseUrls ResponseUrls { get; private set; }
    }
}