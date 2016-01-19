using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal abstract class DigipostAction
    {
        private readonly Uri _signatureServiceRoot;
        private HttpClient _httpClient;
        private readonly object _threadLock = new object();

        public ClientConfiguration ClientConfig { get; set; }

        public X509Certificate2 BusinessCertificate { get; set; }

        public XmlDocument RequestContentXml { get; internal set; }

        public IRequestContent RequestContent { get; set; }


        protected DigipostAction(IRequestContent requestContent, X509Certificate2 businessCertificate, Uri signatureServiceRoot)
        {
            RequestContent = requestContent;
            InitializeRequestXmlContent();
            _signatureServiceRoot = signatureServiceRoot;
            BusinessCertificate = businessCertificate;
        }

        internal HttpClient ThreadSafeHttpClient
        {
            
            get
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                lock (_threadLock)
                {
                    if (_httpClient == null)
                    {
                        var mutualTlsHandler = MutualTlsHandler();
                        var loggingHandler = new LoggingHandler(mutualTlsHandler);

                        _httpClient = new HttpClient(loggingHandler)
                        {
                            Timeout = TimeSpan.FromMilliseconds(5000),
                            BaseAddress = _signatureServiceRoot
                        };
                    }

                    return _httpClient;
                }
            }

            set
            {
                lock (_threadLock)
                {
                    _httpClient = value;
                }
            }
        }

        private WebRequestHandler MutualTlsHandler()
        {
            var certificateCollection = new X509Certificate2Collection() {BusinessCertificate};
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

        internal Task<HttpResponseMessage> PostAsync(Uri requestUri)
        {
            //Todo: Log request starting
            try
            {
                return ThreadSafeHttpClient.PostAsync(requestUri, Content());
                
            }
            finally
            {
                //Todo: Log request ending
            }
        }

        internal Task<HttpResponseMessage> GetAsync()
        {
            try
            {
                //Todo: Log request starting
                return ThreadSafeHttpClient.GetAsync(_signatureServiceRoot);
            }
            finally
            {
                //Todo: Log request ending
            }
        }

        protected abstract HttpContent Content();

        protected abstract string Serialize();

        private void InitializeRequestXmlContent()
        {
            if (RequestContent == null) return;

            var document = new XmlDocument();
            document.LoadXml(Serialize());
            RequestContentXml = document;
        }
    }
}
