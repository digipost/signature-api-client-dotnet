using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal;
using log4net;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.TestClient
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            Console.WriteLine("Starting dat loggah ...");
            Log.Debug("Debug logging");
            Log.Info("Info logging");
            Log.Warn("Warn logging");
            Log.Error("Error logging");
            Log.Fatal("Fatal logging");
            var client = new PortalClient(new ClientConfiguration(Environment.DifiQa, new X509Certificate2(), new Sender("123456789")));
            Console.WriteLine("Finished with loggah ...");
            Console.ReadLine();
        }
    }
}