using System;
using System.IO;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Internal.Extensions;

namespace Digipost.Signature.Api.Client.Core
{
    public class Document : IAsiceAttachable
    {
        public Document(string title, FileType fileType, string documentPath)
            : this(title, fileType, File.ReadAllBytes(documentPath))
        {
        }

        public Document(string title, FileType fileType, byte[] documentBytes)
        {
            Title = title;
            FileName = $"{Guid.NewGuid()}{fileType.GetExtension()}";
            MimeType = fileType.ToMimeType();
            Bytes = documentBytes;
            Id = "Id_" + Guid.NewGuid();
        }

        public string Title { get; }
        
        public string FileName { get; }

        public string Id { get; }

        public string MimeType { get; }

        public byte[] Bytes { get; }

        public override string ToString()
        {
            return $"Title: {Title}, Id: {Id}, MimeType: {MimeType}";
        }
    }
}
