namespace Digipost.Signature.Api.Client.Archive
{
    public class ArchiveDocumentId
    {
        public ArchiveDocumentId(string archiveDocumentId)
        {
            DocumentId = archiveDocumentId;
        }
        public string DocumentId { get; set; }
    }
}
