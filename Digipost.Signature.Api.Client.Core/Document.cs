using System.IO;

namespace Digipost.Signature.Api.Client.Core
{
    public class Document
    {
        public string Subject { get; }
        public string Message { get; }
        public string FileName { get; set; }
        public FileType FileType { get; set; }
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
