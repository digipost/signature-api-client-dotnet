using System.IO;
using System.Text;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility.Utilities;

namespace Digipost.Signature.Api.Client.Core.Tests.Utilities
{
    public static class CoreResponseUtility
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Signature.Api.Client.Core.Tests.Schema.Examples.Response");

        public static string GetBrokerNotAuthorizedError()
        {
            return GetXml("BrokerNotAuthorizedErrorResponse.xml").OuterXml;
        }

        public static string GetError()
        {
            return GetXml("ErrorResponse.xml").OuterXml;
        }

        public static Stream GetXades()
        {
            return new MemoryStream(GetContentBytes("Xades.xml"));
        }

        public static Stream GetPades()
        {
            return new MemoryStream(GetContentBytes("Pades.pdf"));
        }

        internal static XmlDocument GetXml(string kvittering)
        {
            return XmlUtility.TilXmlDokument(GetContent(kvittering));
        }

        private static byte[] GetContentBytes(string path)
        {
            return ResourceUtility.ReadAllBytes(true, path);
        }

        internal static string GetContent(string path)
        {
            return Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(true, path));
        }
    }
}