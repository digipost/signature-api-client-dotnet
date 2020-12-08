using System;
using System.IO;
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
            
            var projectParentDirectory = AppDomain.CurrentDomain.BaseDirectory.Split(new[] {"Digipost.Signature.Api.Client"}, StringSplitOptions.None)[0];
            var nLogConfigPath = Path.Combine(projectParentDirectory, "Digipost.Signature.Api.Client.Core", "nlog.config");
            NLog.LogManager.LoadConfiguration(nLogConfigPath);

            return serviceProvider;
        }
    }
}
