using System;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Xunit;
using Xunit.Sdk;

namespace Digipost.Signature.Api.Client.Archive.Tests.Smoke
{
    public class TestHelper : TestHelperBase
    {
        private readonly ArchiveClient _archiveClient;
        public TestHelper(ArchiveClient archiveClient)
        {
            _archiveClient = archiveClient;
        }
        
        public TestHelper Get_PAdES(string reference)
        {
            var pades = _archiveClient.GetPades(reference).Result;
            Assert.True(pades.CanRead);
            return this;
        }

        public void Download_pades_and_expect_server_error(string archivedDocumentId)
        {
            Assert.Throws<AggregateException>(() => Get_PAdES(archivedDocumentId));
        }
    }

}
