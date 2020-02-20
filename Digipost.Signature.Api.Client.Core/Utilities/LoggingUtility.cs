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
            services.AddLogging((builder) =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true});
            });

            var serviceProvider = services.BuildServiceProvider();
            
            var projectName = new[]{"signature-api-client-dotnet"};
            var projectParentDirectory = System.AppDomain.CurrentDomain.BaseDirectory.Split(projectName, StringSplitOptions.None)[0];
            var nLogConfigPath = projectParentDirectory + $"/{projectName.ElementAt(0)}/Digipost.Signature.Api.Client.Core/nlog.config";
            NLog.LogManager.LoadConfiguration(nLogConfigPath);

            return serviceProvider;
        }
    }
}
