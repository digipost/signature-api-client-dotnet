using System.IO;
using System.Reflection;
using System.Xml;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Digipost.Signature.Api.Client.Scripts.XsdToCode.Code
{
    internal class XsdFiles
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "Digipost.Signature.Api.Client.Scripts.XsdToCode.Xsd");

        internal static XmlReader GetDirect()
        {
            return GetResource("direct.xsd");
        }

        internal static XmlReader GetPortal()
        {
            return GetResource("portal.xsd");
        }

        internal static XmlReader GetCommon()
        {
            return GetResource("common.xsd");
        }

        private static XmlReader GetResource(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}