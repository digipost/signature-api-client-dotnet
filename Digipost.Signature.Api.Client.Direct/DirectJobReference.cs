using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectJobReference
    {
        public Uri Reference { get; set; }

        public DirectJobReference(Uri reference)
        {
            Reference = reference;
        }
    }
}
