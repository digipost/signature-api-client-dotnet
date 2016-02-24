using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Core
{
    public abstract class BaseClient
    {
        protected BaseClient(ClientConfiguration clientConfiguration)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ClientConfiguration = clientConfiguration;
            HttpClient = MutualTlsClient();
        }

        public ClientConfiguration ClientConfiguration { get; }

        internal HttpClient HttpClient { get; set; }

        private HttpClient MutualTlsClient()
        {
            var mutualTlsHandler = MutualTlsHandler();
            var userAgentHttpHandler = new UserAgentHttpHandler(mutualTlsHandler);
            var loggingHandler = new LoggingHandler(userAgentHttpHandler);

            return new HttpClient(loggingHandler)
            {
                Timeout = TimeSpan.FromMilliseconds(5000),
                BaseAddress = ClientConfiguration.Environment.Url
            };
        }

        private WebRequestHandler MutualTlsHandler()
        {
            var certificateCollection = new X509Certificate2Collection {ClientConfiguration.Certificate};
            var mutualTlsHandler = new WebRequestHandler();
            mutualTlsHandler.ClientCertificates.AddRange(certificateCollection);
            mutualTlsHandler.ServerCertificateValidationCallback = ValidateServerCertificate;
            var v = mutualTlsHandler.AuthenticationLevel;

            return mutualTlsHandler;
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            var isValid = false;

           
            
            if((sslpolicyerrors & SslPolicyErrors.RemoteCertificateNameMismatch) != 0)

            if (certificate == null || chain == null)
                isValid = false;



            return true;
        }
    }
}