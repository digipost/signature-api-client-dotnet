using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class StatusReference
    {
        public StatusReference(Uri url)
        {
            Url = url;
        }

        public Uri Url { get; internal set; }
    }
}