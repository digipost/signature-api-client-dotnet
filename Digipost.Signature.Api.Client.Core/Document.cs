using System.IO;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Extensions;

namespace Digipost.Signature.Api.Client.Core
{
    public class Document : IAsiceAttachable
    {
        public string Subject { get; private set; }

        public string Message { get; private set; }

        public string FileName { get; private set; }

        public string Id
        {
            get { return "Id_0"; }
        }

        public string MimeType { get; private set; }

        public byte[] Bytes { get; private set; }

        public Document(string subject, string message, string fileName, FileType fileType, byte[] documentBytes)
        {
            Subject = subject;
            Message = message;
            FileName = fileName;
            MimeType = fileType.ToMimeType();
            Bytes = documentBytes;
        }
        
        public Document(string subject, string message, string fileName, FileType fileType, string documentPath)
            : this(subject, message, fileName, fileType, File.ReadAllBytes(documentPath))
        {
        }
    }
}
