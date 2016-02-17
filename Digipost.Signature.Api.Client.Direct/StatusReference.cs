using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class StatusReference
    {
        public Uri Url { get; internal set; }

        public StatusReference(Uri url)
        {
            Url = url;
        }
    }
}
