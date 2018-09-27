using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Digipost.Signature.Api.Client.Program
{
    public class PortalClient
    {
        private readonly ITestService _testService;
        private readonly IOptions<AppSettings> _config;
        private readonly ILogger<PortalClient> _logger;

        public PortalClient(ITestService testService,
            IOptions<AppSettings> config,
            ILogger<PortalClient> logger)
        {
            _testService = testService;
            _config = config;
            _logger = logger;
            
        }

        public void SendJob()
        {
            _logger.LogInformation($"This is a console application for {_config.Value.Title}!");
            _testService.Run();
            System.Console.ReadKey();
        }
    }
}
