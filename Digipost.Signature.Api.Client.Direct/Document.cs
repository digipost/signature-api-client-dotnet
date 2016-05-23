using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Document : AbstractDocument
    {
        public Document(string title, string message, string fileName, FileType fileType, byte[] documentBytes)
            : base(title, message, fileName, fileType, documentBytes)
        {
        }

        public Document(string title, string message, string fileName, FileType fileType, string documentPath)
            : base(title, message, fileName, fileType, documentPath)
        {
        }
    }
}