using System.IO;
using System.Xml;
using ApiClientShared;

namespace Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code
{
    internal class XsdFiles
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Signature.Api.Client.Scripts.XsdToCode.Xsd");

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
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}