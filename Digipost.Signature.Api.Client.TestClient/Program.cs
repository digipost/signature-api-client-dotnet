using System;
using log4net;

namespace Digipost.Signature.Api.Client.TestClient
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        static void Main(string[] args)
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
