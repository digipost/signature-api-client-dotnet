using System.IO;
using System.Xml;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Digipost.Signature.Api.Client.Resources.Xsd
{
    internal static class XsdResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Signature.Api.Client.Resources.Xsd.Data");

        private static XmlReader GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }

        public static class Signature
        {
            public static XmlReader GetTsXsd()
            {
                return GetResource("ts_102918v010201.xsd");
            }

            public static XmlReader GetXadesXsd()
            {
                return GetResource("XAdES.xsd");
            }

            public static XmlReader GetXmldsigCoreXsd()
            {
                return GetResource("xmldsig-core-schema.xsd");
            }
        }
    }
}