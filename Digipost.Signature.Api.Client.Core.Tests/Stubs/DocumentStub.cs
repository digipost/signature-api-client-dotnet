namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    internal class DocumentStub : AbstractDocument
    {
        public DocumentStub(string title, string message, FileType fileType, byte[] documentBytes)
            : base(title, message, fileType, documentBytes)
        {
        }

        public DocumentStub(string title, string message, FileType fileType, string documentPath)
            : base(title, message, fileType, documentPath)
        {
        }
    }
}
