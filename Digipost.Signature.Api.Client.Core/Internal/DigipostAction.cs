using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal abstract class DigipostAction
    {
        private readonly string _uri;
        private HttpClient _httpClient;

        public ClientConfiguration ClientConfig { get; set; }

        public X509Certificate2 BusinessCertificate { get; set; }
        public XmlDocument RequestContent { get; internal set; }

        private readonly object _threadLock = new object();

        protected DigipostAction(IRequestContent requestContent, ClientConfiguration clientConfig, X509Certificate2 businessCertificate, string uri)
        {
            InitializeRequestXmlContent(requestContent);
            _uri = uri;
            ClientConfig = clientConfig;
            BusinessCertificate = businessCertificate;
        }

        internal HttpClient ThreadSafeHttpClient
        {
            get
            {
                lock (_threadLock)
                {
                    if (_httpClient != null) return _httpClient;

                    var loggingHandler = new LoggingHandler(new HttpClientHandler());

                    _httpClient = new HttpClient(loggingHandler)
                    {
                        Timeout = TimeSpan.FromMilliseconds(5000),
                        BaseAddress = new Uri(ClientConfig.SignatureServiceRoot.AbsoluteUri)
                    };

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

        internal Task<HttpResponseMessage> PostAsync(IRequestContent requestContent)
        {
            //Todo: Log request starting
            try
            {
                return ThreadSafeHttpClient.PostAsync(_uri, Content(requestContent));
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
                return ThreadSafeHttpClient.GetAsync(_uri);
            }
            finally
            {
                //Todo: Log request ending
            }
        }

        protected abstract HttpContent Content(IRequestContent requestContent);

        protected abstract string Serialize(IRequestContent requestContent);

        private void InitializeRequestXmlContent(IRequestContent requestContent)
        {
            if (requestContent == null) return;

            var document = new XmlDocument();
            var serialized = Serialize(requestContent);
            document.LoadXml(serialized);
            RequestContent = document;
        }
    }
}
