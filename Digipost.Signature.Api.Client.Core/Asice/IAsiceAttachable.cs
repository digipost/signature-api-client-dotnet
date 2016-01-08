namespace Digipost.Signature.Api.Client.Core.Asice
{
    public interface IAsiceAttachable
    {
        string FileName { get; }

        byte[] Bytes { get; }

        FileType FileType { get; }

        string Id { get; }

        string MimeType { get; }
    }
}
