namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJobResponse
    {
        public long JobId { get; }

        public ResponseUrls ResponseUrls { get;}

        public DirectJobResponse(long jobId, ResponseUrls responseUrls)
        {
            JobId = jobId;
            ResponseUrls = responseUrls;
        }

        public DirectJobReference DirectJobReference
        {
            get
            {
                return new DirectJobReference(ResponseUrls.Status);
            }
        }
    }


}
