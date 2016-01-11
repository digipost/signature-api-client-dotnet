using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJob
    {
        public string Reference { get;}
        public Signer Signer { get;}
        public Document Document { get;}
        public ExitUrls ExitUrls { get;}

        public DirectJob(string reference, Signer signer, Document document, ExitUrls exitUrls)
        {
            Reference = reference;
            Signer = signer;
            Document = document;
            ExitUrls = exitUrls;
        }
    }
}
