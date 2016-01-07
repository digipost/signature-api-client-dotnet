using System;

namespace Digipost.Signature.Api.Client.Asice
{
    public interface IAsiceAttachable
    {
        string FileName { get; }

        byte[] Bytes { get; }

        string MimeType { get; }
    }
}
