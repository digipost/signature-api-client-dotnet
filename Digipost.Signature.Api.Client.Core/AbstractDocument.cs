using System.IO;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Extensions;

namespace Digipost.Signature.Api.Client.Core
{
    public abstract class AbstractDocument : IAsiceAttachable
    {
        protected AbstractDocument(string title, string message, string fileName, FileType fileType, byte[] documentBytes)
        {
            Title = title;
            Message = message;
            FileName = fileName;
            MimeType = fileType.ToMimeType();
            Bytes = documentBytes;
        }

        protected AbstractDocument(string title, string message, string fileName, FileType fileType, string documentPath)
            : this(title, message, fileName, fileType, File.ReadAllBytes(documentPath))
        {
        }

        public string Title { get; private set; }

        public string Message { get; private set; }

        public string FileName { get; }

        public string Id => "Id_0";

        public string MimeType { get; }

        public byte[] Bytes { get; }
    }
}