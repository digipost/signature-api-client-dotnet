using System;

namespace Digipost.Signature.Api.Client.Core
{
    public class ConfirmationReference
    {
        public ConfirmationReference(Uri url)
        {
            Url = url;
        }

        public Uri Url { get; set; }
    }
}