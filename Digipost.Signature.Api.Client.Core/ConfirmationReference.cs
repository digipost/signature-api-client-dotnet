using System;

namespace Digipost.Signature.Api.Client.Core
{
    public class ConfirmationReference
    {
        public Uri ConfirmationUri { get; set; }

        public ConfirmationReference(Uri confirmationUri)
        {
            ConfirmationUri = confirmationUri;
        }
    }
}
