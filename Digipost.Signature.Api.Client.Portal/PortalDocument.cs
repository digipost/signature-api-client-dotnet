using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalDocument : Document
    {
        public string NonsensitiveTitle { get; set; }

        public PortalDocument(string title, string message, string fileName, FileType fileType, byte[] documentBytes)
            : base(title, message, fileName, fileType, documentBytes)
        {
        }

        public PortalDocument(string title, string message, string fileName, FileType fileType, string documentPath)
            : base(title, message, fileName, fileType, documentPath)
        {
        }
    }
}
