namespace Digipost.Signature.Api.Client.Archive
{
    public class ArchivedDocumentId
    {
        public ArchivedDocumentId(string documentId)
        {
            Value = documentId;
        }
        public string Value { get; }
    }
}
