using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJob
    {
        public string Id { get;}
        public Signer Signer { get;}
        public Document Document { get;}
        public ExitUrls ExitUrls { get;}

        public DirectJob(string id, Signer signer, Document document, ExitUrls exitUrls)
        {
            Id = id;
            Signer = signer;
            Document = document;
            ExitUrls = exitUrls;
        }
    }
}
