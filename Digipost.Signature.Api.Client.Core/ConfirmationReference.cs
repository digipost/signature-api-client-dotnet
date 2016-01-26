using System;

namespace Digipost.Signature.Api.Client.Core
{
    public class ConfirmationReference
    {
        public Uri ConfirmationUrl { get; private set; }

        public ConfirmationReference(Uri confirmationUrl)
        {
            ConfirmationUrl = confirmationUrl;
        }
    }
}