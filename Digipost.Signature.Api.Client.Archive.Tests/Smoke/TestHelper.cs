using System;
using System.Reflection.Metadata;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Xunit;

namespace Digipost.Signature.Api.Client.Archive.Tests.Smoke
{
    public class TestHelper : TestHelperBase
    {
        private readonly ArchiveClient _archiveClient;
        private readonly DocumentOwner _documentOwner;
        private readonly ArchiveDocumentId _documentId;
        public TestHelper(ArchiveClient archiveClient, DocumentOwner documentOwner, ArchiveDocumentId documentId)
        {
            _archiveClient = archiveClient;
            _documentOwner = documentOwner;
            _documentId = documentId;
        }
        
        public TestHelper Get_PAdES()
        {
            var pades = _archiveClient.GetPades(_documentOwner, _documentId).Result;
            Assert.True(pades.CanRead);
            return this;
        }

        public void Download_pades_and_expect_client_error()
        {
            try
            {
                Get_PAdES();
            }
            catch (AggregateException ex)
            {
                if(ex.InnerException is UnexpectedResponseException)
                {
                    UnexpectedResponseException exception = (UnexpectedResponseException) ex.InnerException;
                    Assert.Equal("ARCHIVED_DOCUMENT_NOT_FOUND", exception.Error.Code);
                }
                else
                {
                    Assert.True(false, $"Expected UnexpectedResponseException, but found {ex.InnerException.GetType().Name}.");
                }
            }

            
        }
    }

}
