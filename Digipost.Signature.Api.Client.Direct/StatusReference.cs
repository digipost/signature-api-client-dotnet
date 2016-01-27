using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class StatusReference
    {
        public Uri Reference { get; set; }

        public StatusReference(Uri reference)
        {
            Reference = reference;
        }
    }
}
