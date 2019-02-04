using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Digipost.Signature.Api.Client.Core
{ 
    public class CertificateReader2
    {
        private ILogger<CertificateReader2> _logger;

        public CertificateReader2(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CertificateReader2>();
        }
        
        public X509Certificate2 ReadCertificate()
        {
            var pathToSecrets = $"{System.Environment.GetEnvironmentVariable("HOME")}/.microsoft/usersecrets/organization-certificate/secrets.json";
            _logger.LogDebug($"Reading certificate details from secrets file: {pathToSecrets}");
            var fileExists = File.Exists(pathToSecrets);

            if (!fileExists)
            {
                _logger.LogDebug($"Did not find file at {pathToSecrets}");
            }
            
            var certificateConfig = File.ReadAllText(pathToSecrets);
            var deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(certificateConfig);

            deserializeObject.TryGetValue("Certificate:Path:Absolute", out var certificatePath);
            deserializeObject.TryGetValue("Certificate:Password", out var certificatePassword);

            _logger.LogDebug("Reading certificate from path found in secrets file: " + certificatePath);

            return new X509Certificate2(certificatePath, certificatePassword, X509KeyStorageFlags.Exportable);
        }
    }
}
