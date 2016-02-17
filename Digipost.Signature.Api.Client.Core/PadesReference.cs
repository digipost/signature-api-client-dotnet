using System;

namespace Digipost.Signature.Api.Client.Core
{
    public class PadesReference
    {
        public Uri Url { get; }

        public PadesReference(Uri url)
        {
            Url = url;
        }
    }
}
