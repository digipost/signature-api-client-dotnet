using ApiClientShared;

namespace Digipost.Signature.Api.Client.Core
{
    public static class DomainUtility
    {
        static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Signature.Api.Client.CoreTests.Resources.Documents");

        public static Document GetDocument()
        {
            return new Document("Testdocument", "A test document from domain Utility", "TestFileName", FileType.Pdf, GetPdfDocumentBytes());
        }

        public static byte[] GetPdfDocumentBytes()
        {
            return ResourceUtility.ReadAllBytes(true, "Dokument.pdf");
        }
    }
}
