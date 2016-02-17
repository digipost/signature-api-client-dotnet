using System;

namespace Digipost.Signature.Api.Client.Core
{
    public class XadesReference
    {
        public Uri Url { get; }

        public XadesReference(Uri url)
        {
            Url = url;
        }
    }
}
