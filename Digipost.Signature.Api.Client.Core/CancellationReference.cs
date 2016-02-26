using System;

namespace Digipost.Signature.Api.Client.Core
{
    public class CancellationReference
    {
        public CancellationReference(Uri url)
        {
            Url = url;
        }

        public Uri Url { get; }
    }
}