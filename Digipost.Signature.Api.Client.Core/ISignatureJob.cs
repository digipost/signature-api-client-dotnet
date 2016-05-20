namespace Digipost.Signature.Api.Client.Core
{
    public interface ISignatureJob
    {
        Sender Sender { get; }

        AbstractDocument Document { get; }

        string Reference { get; }
    }
}