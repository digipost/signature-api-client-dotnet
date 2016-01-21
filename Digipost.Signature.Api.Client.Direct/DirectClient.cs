using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Direct.Internal;
using System.Net.Security;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectClient
    {
        private HttpClient _httpClient;
        private static readonly Uri DirectJobSubPath = new Uri("/api/signature-jobs", UriKind.Relative);

        public ClientConfiguration ClientConfiguration { get; }

        public DirectClient(ClientConfiguration clientConfiguration)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ClientConfiguration = clientConfiguration;
            HttpClient = MutualTlsClient();
        }

        public async Task<HttpResponseMessage> Create(DirectJob directJob)
        {
            var signers = new List<Signer> {directJob.Signer};
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, directJob.Document, signers, ClientConfiguration.Certificate);
            var createAction = new CreateAction(directJob, documentBundle, ClientConfiguration.Certificate, ClientConfiguration.SignatureServiceRoot);

            var httpResponseMessage = await createAction.PostAsync(HttpClient, DirectJobSubPath);
            return httpResponseMessage;

            //Todo:Return DirectJobResponse instead of HttpResponseMessage
        }

        public DirectJobStatusResponse GetStatus(DirectJobReference directJobReference)
        {
            throw new NotImplementedException();
        }

        public Stream GetXades(XadesReference xadesReference)
        {
            throw new NotImplementedException();
        }

        public Stream GetPades(PadesReference xadesReference)
        {
            throw new NotImplementedException();
        }

        internal HttpClient HttpClient { get; set; }

        private HttpClient MutualTlsClient()
        {
            var mutualTlsHandler = MutualTlsHandler();
            var loggingHandler = new LoggingHandler(mutualTlsHandler);

            _httpClient = new HttpClient(loggingHandler)
            {
                Timeout = TimeSpan.FromMilliseconds(5000),
                BaseAddress = ClientConfiguration.SignatureServiceRoot
            };

            return _httpClient;
        }

        private WebRequestHandler MutualTlsHandler()
        {
            var certificateCollection = new X509Certificate2Collection() { ClientConfiguration.Certificate };
            var mutualTlsHandler = new WebRequestHandler();
            mutualTlsHandler.ClientCertificates.AddRange(certificateCollection);
            mutualTlsHandler.ServerCertificateValidationCallback = ValidateServerCertificate;

            return mutualTlsHandler;
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            //Todo: Implement server certificate validation
            return true;
        }



    }
}
