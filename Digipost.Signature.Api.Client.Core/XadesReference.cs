using System;

namespace Digipost.Signature.Api.Client.Core
{
    public class XadesReference
    {
        public Uri XadesUri { get; }

        public XadesReference(Uri xadesUri)
        {
            XadesUri = xadesUri;
        }
    }
}
