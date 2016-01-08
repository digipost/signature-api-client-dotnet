using System.IO;
using Digipost.Signature.Api.Client.Core.Asice;

namespace Digipost.Signature.Api.Client.Core
{
    public class Document : IAsiceAttachable
    {
        public string Subject { get; }

        public string Message { get; }

        public string FileName { get; set; }

        public FileType FileType { get; set; }

        public string Id { get; }

        public string MimeType { get; }

        public byte[] Bytes { get; set; }

        public Document(string subject, string message, string fileName, FileType fileType, byte[] documentBytes)
        {
            Subject = subject;
            Message = message;
            FileName = fileName;
            FileType = fileType;
            Bytes = documentBytes;
        }
        
        public Document(string subject, string message, string fileName, FileType fileType, string documentPath)
            : this(subject, message, fileName, fileType, File.ReadAllBytes(documentPath))
        {
        }
    }
}
