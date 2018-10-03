using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace Digipost.Signature.Api.Client.Core
{
    public static class CertificateReader
    {
        private static X509Certificate2 _certificate;

        public static X509Certificate2 GetCertificate()
        {
            return _certificate ?? (_certificate = ReadCertificate());
        }

        private static X509Certificate2 ReadCertificate()
        {
            var pathToSecrets = $"{System.Environment.GetEnvironmentVariable("HOME")}/.microsoft/usersecrets/organization-certificate/secrets.json";
            Console.WriteLine($"Reading certificate details from secrets file: {pathToSecrets}");
            var certificateConfig = File.ReadAllText(pathToSecrets);
            var deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(certificateConfig);

            deserializeObject.TryGetValue("Certificate:Path:Absolute", out var certificatePath);
            deserializeObject.TryGetValue("Certificate:Password", out var certificatePassword);

            Console.WriteLine("Reading certificate from path found in secrets file: " + certificatePath);

            return new X509Certificate2(certificatePath, certificatePassword, X509KeyStorageFlags.Exportable);
        }
    }
}