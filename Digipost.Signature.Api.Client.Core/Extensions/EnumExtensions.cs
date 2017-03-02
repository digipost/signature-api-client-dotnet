using System;
using Digipost.Signature.Api.Client.Core.Enums;

namespace Digipost.Signature.Api.Client.Core.Extensions
{
    public static class EnumExtensions
    {
        public static signaturetype ToSignaturtype(this SignatureType signatureType)
        {
            switch (signatureType)
            {
                case SignatureType.AdvancedSignature:
                    return signaturetype.ADVANCED_ELECTRONIC_SIGNATURE;
                case SignatureType.AuthenticatedSignature:
                    return signaturetype.AUTHENTICATED_ELECTRONIC_SIGNATURE;
                default:
                    throw new ArgumentOutOfRangeException(nameof(signatureType), signatureType, null);
            }
        }

        public static authenticationlevel ToAuthenticationlevel(this AuthenticationLevel authenticationLevel)
        {
            switch (authenticationLevel)
            {
                case AuthenticationLevel.Three:
                    return authenticationlevel.Item3;
                case AuthenticationLevel.Four:
                    return authenticationlevel.Item4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(authenticationLevel), authenticationLevel, null);
            }
        }

        public static signingonbehalfof ToSigningonbehalfof(this OnBehalfOf onBehalfOf)
        {
            switch (onBehalfOf)
            {
                case OnBehalfOf.Self:
                    return signingonbehalfof.SELF;
                case OnBehalfOf.Other:
                    return signingonbehalfof.OTHER;
                default:
                    throw new ArgumentOutOfRangeException(nameof(onBehalfOf), onBehalfOf, null);
            }
        }
    }
}