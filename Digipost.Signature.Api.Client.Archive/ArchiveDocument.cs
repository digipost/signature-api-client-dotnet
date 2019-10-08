namespace Digipost.Signature.Api.Client.Archive
{
    public class ArchiveDocument
    {
        public ArchiveDocument(string documentId)
        {
            Id = documentId;
        }
        public string Id { get; }
    }
}
