namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJobResponse
    {
        public DirectJobResponse(long jobId, ResponseUrls responseUrls)
        {
            JobId = jobId;
            ResponseUrls = responseUrls;
        }

        public long JobId { get; private set; }

        public ResponseUrls ResponseUrls { get; private set; }
    }
}