using System;
using System.IO;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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
    }
}
