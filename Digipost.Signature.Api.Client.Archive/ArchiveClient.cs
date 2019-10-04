using System;
using System.IO;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.Archive
{
    public class ArchiveClient : BaseClient
    {
        private readonly ILogger<BaseClient> _logger;

        public ArchiveClient(ClientConfiguration clientConfiguration)
            : this(clientConfiguration, new NullLoggerFactory())
        {
        }
        
        public ArchiveClient(ClientConfiguration clientConfiguration, ILoggerFactory loggerFactory)
            : base(clientConfiguration, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ArchiveClient>();
        }

        public async Task<Stream> GetPades(DocumentOwner owner, string archiveDocumentId)
        {
            var uri = new Uri($"{ClientConfiguration.Environment.Url}api/{owner.OrganizationNumber}/archive/documents/{archiveDocumentId}/pades"); 
            return await RequestHelper.GetStream(uri).ConfigureAwait(false);
        }
    }
}
