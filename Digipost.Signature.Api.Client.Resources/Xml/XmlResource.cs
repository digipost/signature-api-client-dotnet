using System.Reflection;
using System.Text;
using System.Xml;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Digipost.Signature.Api.Client.Resources.Xml
{
    internal class XmlResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "Digipost.Signature.Api.Client.Resources.Xml.Data");

        private static XmlDocument GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytes(path);
            return XmlUtility.ToXmlDocument(Encoding.UTF8.GetString(bytes));
        }

        internal class Request
        {
            public static XmlDocument GetPortalManifest()
            {
                return GetResource("Request", "PortalManifest.xml");
            }
        }

        internal class Response
        {
            public static XmlDocument GetTransportError()
            {
                return GetResource("Response", "CreateTransportError.xml");
            }

            public static XmlDocument GetTransportOk()
            {
                return GetResource("Response", "CreateTransportOk.xml");
            }
        }
    }
}