namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    public class SignatureJobStub : ISignatureJob
    {
        public Sender Sender { get; }

        public Core.Document Document { get; }

        public string Reference { get; set; }
    }
}