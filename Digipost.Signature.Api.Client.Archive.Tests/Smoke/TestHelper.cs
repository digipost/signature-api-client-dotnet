using System;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Xunit;

namespace Digipost.Signature.Api.Client.Archive.Tests.Smoke
{
    public class TestHelper : TestHelperBase
    {
        private readonly ArchiveClient _archiveClient;
        public TestHelper(ArchiveClient archiveClient)
        {
            _archiveClient = archiveClient;
        }
        
        public TestHelper Get_PAdES(string documentOwner, string reference)
        {
            DocumentOwner owner = new DocumentOwner(documentOwner);
            var pades = _archiveClient.GetPades(owner, reference).Result;
            Assert.True(pades.CanRead);
            return this;
        }

        public void Download_pades_and_expect_client_error(string archiveDocumentOwner, string archiveDocumentId)
        {
            try
            {
                Get_PAdES(archiveDocumentOwner, archiveDocumentId);
            }
            catch (AggregateException ex)
            {
                if(ex.InnerException is UnexpectedResponseException)
                {
                    UnexpectedResponseException exception = (UnexpectedResponseException) ex.InnerException;
                    Assert.Equal(exception.Error.Code, "ARCHIVED_DOCUMENT_NOT_FOUND");
                }
                else
                {
                    Assert.True(false, $"Expected UnexpectedResponseException, but found {ex.InnerException.GetType().Name}.");
                }
            }

            
        }
    }

}
