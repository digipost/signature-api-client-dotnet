using System;
using System.Net.Http;
using System.Xml;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal abstract class DigipostAction
    {
        public IRequestContent RequestContent { get; }
        public Sender Sender { get; set; }

        public XmlDocument RequestContentXml { get; internal set; }

        protected DigipostAction(IRequestContent requestContent, Sender sender, Func<IRequestContent, Sender, string> serializeFunc)
        {
            RequestContent = requestContent;
            Sender = sender;
            InitializeRequestXmlContent(serializeFunc);
        }

        protected string SerializedBody { get; set; }

        internal abstract HttpContent Content();

        private void InitializeRequestXmlContent(Func<IRequestContent, Sender, string> serializeFunc)
        {
            var document = new XmlDocument();
            SerializedBody = serializeFunc(RequestContent, Sender);
            document.LoadXml(SerializedBody);
            RequestContentXml = document;
        }
    }
}
