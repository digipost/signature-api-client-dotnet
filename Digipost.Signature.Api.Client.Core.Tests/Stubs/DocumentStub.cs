namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    internal class DocumentStub : Document
    {
        public DocumentStub(string title, FileType fileType, byte[] documentBytes)
            : base(title, fileType, documentBytes)
        {
        }

        public DocumentStub(string title, FileType fileType, string documentPath)
            : base(title, fileType, documentPath)
        {
        }
    }
}
