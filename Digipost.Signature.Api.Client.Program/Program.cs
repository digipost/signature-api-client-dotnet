using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Digipost.Signature.Api.Client.Program
{
    class Program
    {
        static void Main(string[] args)
        {
//            ClientConfiguration clientConfiguration = new ClientConfiguration(Environment.Test,  CertificateReader.ReadCertificate());
//            PortalClient portalClient = new PortalClient(clientConfiguration);
//            var serviceProvider = CreateServiceProvider(clientConfiguration);

//            SetUpNLog(serviceProvider);
            
//            new DirectClient()

//            var portalClient = serviceProvider.GetService<PortalClient>();
//            portalClient.GetRootResource(new Sender("988015814"));
        }

        private static void SetUpNLog(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true});
            NLog.LogManager.LoadConfiguration("/Users/aas/projects/digipost/signature-api-client-dotnet/Digipost.Signature.Api.Client.Program/nlog.config");
        }

//        private static IServiceProvider CreateServiceProvider(ClientConfiguration clientConfiguration)
//        {
//            var services = new ServiceCollection();
//            
//            services.AddSingleton<ILoggerFactory, LoggerFactory>();
//            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
//            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));
//            services.AddSingleton(clientConfiguration);
//            services.AddSingleton<PortalClient>();
//
//            return services.BuildServiceProvider();
//        }
    }
}
