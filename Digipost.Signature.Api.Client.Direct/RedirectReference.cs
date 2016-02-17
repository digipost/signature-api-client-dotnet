using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class RedirectReference
    {
        public Uri Url { get; internal set; }

        public RedirectReference(Uri url)
        {
            Url = url;
        }
    }
}