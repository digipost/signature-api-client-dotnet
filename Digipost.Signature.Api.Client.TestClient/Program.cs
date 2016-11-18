using System;
using System.Reflection;
using Common.Logging;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal;
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
            var client = new PortalClient(new ClientConfiguration(Environment.DifiQa, "‎2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed", new Sender("123456789")));

            Console.WriteLine("Finished with loggah ...");
            Console.ReadLine();
        }
    }
}