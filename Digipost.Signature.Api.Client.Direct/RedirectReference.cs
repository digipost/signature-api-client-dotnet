using System;
using Digipost.Signature.Api.Client.Core.Identifier;

namespace Digipost.Signature.Api.Client.Direct
{
    public class RedirectReference
    {
        public RedirectReference(Uri url, PersonalIdentificationNumber signer)
        {
            Url = url;
            Signer = signer;
        }

        public Uri Url { get; internal set; }

        public PersonalIdentificationNumber Signer { get; internal set; }

        public override string ToString()
        {
            return $"Redirect url for signer {Signer}: {Url}";
        }
    }
}
