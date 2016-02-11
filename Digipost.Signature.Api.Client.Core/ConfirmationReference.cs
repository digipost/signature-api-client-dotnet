using System;

namespace Digipost.Signature.Api.Client.Core
{
    public class ConfirmationReference
    {
        public Uri Url { get; set; }

        public ConfirmationReference(Uri url)
        {
            Url = url;
        }
    }
}
