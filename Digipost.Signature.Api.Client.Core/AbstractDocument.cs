using System;
using System.IO;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Internal.Extensions;

namespace Digipost.Signature.Api.Client.Core
{
    public abstract class AbstractDocument : IAsiceAttachable
    {
        protected AbstractDocument(string title, string message, FileType fileType, string documentPath)
            : this(title, message, fileType, File.ReadAllBytes(documentPath))
        {
        }

        protected AbstractDocument(string title, string message, FileType fileType, byte[] documentBytes)
        {
            Title = title;
            Message = message;
            FileName = $"{DateTime.Now.ToString("yyyyMMddssfff")}document";
            MimeType = fileType.ToMimeType();
            Bytes = documentBytes;
        }

        public string Title { get; }

        public string Message { get; }

        public string FileName { get; }

        public string Id => "Id_0";

        public string MimeType { get; }

        public byte[] Bytes { get; }

        public override string ToString()
        {
            return $"Title: {Title}, Message: {Message}, Id: {Id}, MimeType: {MimeType}";
        }
    }
}
