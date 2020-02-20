using System;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Digipost.Signature.Api.Client.Docs.Logging
{
    public class Logging
    {
        private static IServiceProvider CreateServiceProviderAndSetUpLogging()
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
            NLog.LogManager.LoadConfiguration("./nlog.config");

            return serviceProvider;
        }
        
        static void Main(string[] args)
        {
            ClientConfiguration clientConfiguration = null;
            var serviceProvider = CreateServiceProviderAndSetUpLogging();
            var client = new PortalClient(clientConfiguration, serviceProvider.GetService<ILoggerFactory>());
        }
    }
}
