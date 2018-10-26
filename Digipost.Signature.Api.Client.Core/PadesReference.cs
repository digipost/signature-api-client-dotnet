using System;

namespace Digipost.Signature.Api.Client.Core
{
    public class PadesReference
    {
        public PadesReference(Uri url)
        {
            Url = url;
        }

        public Uri Url { get; }

        public override string ToString()
        {
            return $"Url: {Url}";
        }
    }
}
