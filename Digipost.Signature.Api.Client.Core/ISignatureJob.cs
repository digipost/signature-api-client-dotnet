namespace Digipost.Signature.Api.Client.Core
{
    public interface ISignatureJob
    {
        Sender Sender { get; }

        Document Document { get; }

        string Reference { get; }
    }
}