namespace Digipost.Signature.Api.Client.Core.Internal.Asice
{
    public interface IAsiceAttachable
    {
        string FileName { get; }

        byte[] Bytes { get; }

        string Id { get; }

        string MimeType { get; }
    }
}