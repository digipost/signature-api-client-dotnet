namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    internal class DocumentStub : AbstractDocument
    {
        public DocumentStub(string title, string message, string fileName, FileType fileType, byte[] documentBytes)
            : base(title, message, fileName, fileType, documentBytes)
        {
        }

        public DocumentStub(string title, string message, string fileName, FileType fileType, string documentPath)
            : base(title, message, fileName, fileType, documentPath)
        {
        }
    }
}