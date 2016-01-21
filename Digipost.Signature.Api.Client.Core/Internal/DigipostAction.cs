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
        private readonly object _threadLock = new object();
        private HttpClient _httpClient;

        public IRequestContent RequestContent { get; }

        public X509Certificate2 BusinessCertificate { get; }

        public Uri SignatureServiceRoot { get;  }
        public XmlDocument RequestContentXml { get; internal set; }

        protected DigipostAction(IRequestContent requestContent, X509Certificate2 businessCertificate, Uri signatureServiceRoot, Func<IRequestContent,string> serializeFunc)
        {
            RequestContent = requestContent;
            SignatureServiceRoot = signatureServiceRoot;
            BusinessCertificate = businessCertificate;
            InitializeRequestXmlContent(serializeFunc);
        }

        protected string SerializedBody { get; set; }

        protected abstract HttpContent Content();

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
                            BaseAddress = SignatureServiceRoot
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

        internal Task<HttpResponseMessage> PostAsync(Uri requestUri)
        {
            return ThreadSafeHttpClient.PostAsync(requestUri, Content());
        }

        internal Task<HttpResponseMessage> GetAsync()
        {
            return ThreadSafeHttpClient.GetAsync(SignatureServiceRoot);
        }

        private void InitializeRequestXmlContent(Func<IRequestContent, string> serializeFunc)
        {
            var document = new XmlDocument();
            SerializedBody = serializeFunc(RequestContent);
            document.LoadXml(SerializedBody);
            RequestContentXml = document;
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
    }
}
