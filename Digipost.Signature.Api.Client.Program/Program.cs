using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Digipost.Signature.Api.Client.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            // create service provider
            var serviceProvider = CreateServiceProvider();

            SetUpNLog(serviceProvider);

            var testService = serviceProvider.GetService<ITestService>();
            testService.Run();
        }

        private static void SetUpNLog(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true});
            NLog.LogManager.LoadConfiguration("/Users/aas/projects/digipost/signature-api-client-dotnet/Digipost.Signature.Api.Client.Program/nlog.config");
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));
            services.AddTransient<ITestService, TestService>();

            return services.BuildServiceProvider();
        }
    }
}
