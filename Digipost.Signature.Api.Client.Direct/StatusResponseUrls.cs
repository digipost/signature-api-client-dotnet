using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class StatusResponseUrls
    {
        public StatusResponseUrls(Uri confirmation, Uri xades, Uri pades)
        {
            Confirmation = confirmation;
            Xades = xades;
            Pades = pades;
        }

        public Uri Confirmation { get; }

        public Uri Xades{ get; }

        public Uri Pades { get; }

    }
}