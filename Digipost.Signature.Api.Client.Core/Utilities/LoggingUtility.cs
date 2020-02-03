using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Digipost.Signature.Api.Client.Core.Utilities
{
    public static class LoggingUtility
    {
        internal static IServiceProvider CreateServiceProviderAndSetUpLogging()
        {
            var services = new ServiceCollection();
            
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            var serviceProvider = services.BuildServiceProvider();
            SetUpLoggingForTesting(serviceProvider);

            return serviceProvider;
        }
        
        private static void SetUpLoggingForTesting(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true});
            
            var projectName = new[]{"signature-api-client-dotnet"};
            var projectParentDirectory = System.AppDomain.CurrentDomain.BaseDirectory.Split(projectName, StringSplitOptions.None)[0];
            var nLogConfigPath = projectParentDirectory + $"/{projectName.ElementAt(0)}/Digipost.Signature.Api.Client.Core/nlog.config";
            NLog.LogManager.LoadConfiguration(nLogConfigPath);
        }

    }
}
