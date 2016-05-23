namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    public class SignatureJobStub : ISignatureJob
    {
        public Sender Sender { get; }

        public AbstractDocument Document { get; }

        public string Reference { get; set; }
    }
}