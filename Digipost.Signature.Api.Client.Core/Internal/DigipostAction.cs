using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal abstract class DigipostAction
    {
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

        internal Task<HttpResponseMessage> PostAsync(HttpClient httpClient, Uri requestUri)
        {
            return httpClient.PostAsync(requestUri, Content());
        }

        internal Task<HttpResponseMessage> GetAsync(HttpClient httpClient)
        {
            return httpClient.GetAsync(SignatureServiceRoot);
        }

        private void InitializeRequestXmlContent(Func<IRequestContent, string> serializeFunc)
        {
            var document = new XmlDocument();
            SerializedBody = serializeFunc(RequestContent);
            document.LoadXml(SerializedBody);
            RequestContentXml = document;
        }

    }
}
