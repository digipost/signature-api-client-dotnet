using System.IO;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Extensions;

namespace Digipost.Signature.Api.Client.Core
{
    public class Document : IAsiceAttachable
    {
        public string Subject { get; }

        public string Message { get; }

        public string FileName { get; }

        public string Id
        {
            get { return "Id_0"; }
        }

        public string MimeType { get; }

        public byte[] Bytes { get;}

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
