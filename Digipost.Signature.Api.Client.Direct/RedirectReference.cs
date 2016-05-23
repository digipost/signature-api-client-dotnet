using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class RedirectReference
    {
        public RedirectReference(Uri url)
        {
            Url = url;
        }

        public Uri Url { get; internal set; }

        public override string ToString()
        {
            return $"Url: {Url}";
        }
    }
}