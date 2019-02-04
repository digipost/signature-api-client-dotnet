using System;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Direct;
using Digipost.Signature.Api.Client.Portal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Digipost.Signature.Api.Client.Program
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = CreateServiceProvider();

            SetUpNLog(serviceProvider);

            CertificateReader2 certificateReader2 = new CertificateReader2(serviceProvider.GetService<ILoggerFactory>());
            var clientConfiguration = new ClientConfiguration(Core.Environment.Test, certificateReader2.ReadCertificate());
            var portalClient = new PortalClient(clientConfiguration, serviceProvider.GetService<ILoggerFactory>());
            
            var result = portalClient.GetRootResource(new Sender("988015814")).Result;
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

            return services.BuildServiceProvider();
        }
    }
}
