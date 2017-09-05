using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core.Identifier;

namespace Digipost.Signature.Api.Client.Direct
{
    public class RedirectUrls
    {
        public RedirectUrls(List<RedirectReference> urls)
        {
            Urls = urls;
        }

        public List<RedirectReference> Urls { get; set; }

        /// <summary>
        ///     Gets the redirect URL for a given signer.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the job response doesn't contain a redirect URL for this signer</exception>
        /// <seealso cref="ResponseUrls.GetSingleRedirectUrl" />
        public RedirectReference GetFor(PersonalIdentificationNumber personalIdentificationNumber)
        {
            var redirectReference = Urls.SingleOrDefault(url => url.Signer.Equals(personalIdentificationNumber));
            if (redirectReference == null)
            {
                throw new InvalidOperationException("Unable to find redirect URL for this signer");
            }
            return redirectReference;
        }

        internal RedirectReference GetSingleRedirectUrl()
        {
            if (Urls.Count() != 1)
            {
                throw new InvalidOperationException("Calls to this method should only be done when there are no more than one (1) redirect URL.");
            }
            return Urls.Single();
        }
    }
}