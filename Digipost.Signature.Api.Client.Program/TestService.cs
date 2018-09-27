using System;
using Microsoft.Extensions.Logging;

namespace Digipost.Signature.Api.Client.Program
{
    public interface ITestService
    {
        void Run();
    }

    public class TestService : ITestService
    {
        private readonly ILogger<ITestService> _logger;

        public TestService(ILogger<TestService> logger)
        {
            _logger = logger;
        }
        
        public void Run()
        {
            _logger.LogWarning("Dis is inzane! We are in the test service");
        }
    }
}
