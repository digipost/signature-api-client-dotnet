using System;

namespace Digipost.Signature.Api.Client.Direct.NewRedirectUrl
{
    public interface IWithSignerUrl
    {
        Uri SignerUrl { get; }
    }
}
