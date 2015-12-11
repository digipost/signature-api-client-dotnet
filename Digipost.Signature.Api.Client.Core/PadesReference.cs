using System;

namespace Digipost.Signature.Api.Client.Core
{
    public class PadesReference
    {
        public Uri PadesUri { get; }

        public PadesReference(Uri padesUri)
        {
            PadesUri = padesUri;
        }
    }
}
