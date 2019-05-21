using System;
using Digipost.Signature.Api.Client.Core.Identifier;
using Schemas;

namespace Digipost.Signature.Api.Client.Direct
{
    public class SignerResponse
    {
        public SignerIdentifier Identifier { get; }

        public Uri RedirectUrl { get; }

        public Uri SignerUrl { get; }

        public SignerResponse(SignerIdentifier identifier, Uri redirectUrl, Uri signerUrl)
        {
            Identifier = identifier;
            RedirectUrl = redirectUrl;
            SignerUrl = signerUrl;
        }

        internal SignerResponse(directsignerresponse signerResponse)
        {
            switch (signerResponse.ItemElementName)
            {
                case ItemChoiceType1.personalidentificationnumber:
                    Identifier = new PersonalIdentificationNumber(signerResponse.Item);
                    break;
                case ItemChoiceType1.signeridentifier:
                    Identifier = new CustomIdentifier(signerResponse.Item);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            RedirectUrl = new Uri(signerResponse.redirecturl);
            SignerUrl = new Uri(signerResponse.href);
        }

        public override bool Equals(object obj)
        {
            return obj is SignerResponse that
                   && Identifier.IsSameAs(that.Identifier)
                   && RedirectUrl.Equals(that.RedirectUrl);
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode() + RedirectUrl.GetHashCode();
        }
    }
}
