using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Document : AbstractDocument
    {
        public Document(string title, string message, FileType fileType, byte[] documentBytes)
            : base(title, message, fileType, documentBytes)
        {
        }

        public Document(string title, string message, FileType fileType, string documentPath)
            : base(title, message, fileType, documentPath)
        {
        }
    }
}