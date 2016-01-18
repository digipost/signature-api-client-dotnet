using System;
using System.Net.Http;
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
            //Todo: Set TLS2 settings like in example project
            get
            {
                lock (_threadLock)
                {
                    if (_httpClient != null) return _httpClient;

                    var loggingHandler = new LoggingHandler(new HttpClientHandler());

                    _httpClient = new HttpClient(loggingHandler)
                    {
                        Timeout = TimeSpan.FromMilliseconds(5000),
                        BaseAddress = new Uri(_signatureServiceRoot.AbsoluteUri)
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

        internal Task<HttpResponseMessage> PostAsync()
        {
            //Todo: Log request starting
            try
            {
                return ThreadSafeHttpClient.PostAsync(_signatureServiceRoot, Content());
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
