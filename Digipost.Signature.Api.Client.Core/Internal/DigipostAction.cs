using System;
using System.Net.Http;
using System.Xml;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal abstract class DigipostAction
    {
        public IRequestContent RequestContent { get; }

        public XmlDocument RequestContentXml { get; internal set; }

        protected DigipostAction(IRequestContent requestContent, Func<IRequestContent,string> serializeFunc)
        {
            RequestContent = requestContent;
            InitializeRequestXmlContent(serializeFunc);
        }

        protected string SerializedBody { get; set; }

        internal abstract HttpContent Content();

        private void InitializeRequestXmlContent(Func<IRequestContent, string> serializeFunc)
        {
            var document = new XmlDocument();
            SerializedBody = serializeFunc(RequestContent);
            document.LoadXml(SerializedBody);
            RequestContentXml = document;
        }

    }
}
