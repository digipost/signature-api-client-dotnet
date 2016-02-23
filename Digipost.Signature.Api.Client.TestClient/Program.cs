using System;
using System.Reflection;
using log4net;

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
            Console.WriteLine("Finished with loggah ...");
            Console.ReadLine();
        }
    }
}