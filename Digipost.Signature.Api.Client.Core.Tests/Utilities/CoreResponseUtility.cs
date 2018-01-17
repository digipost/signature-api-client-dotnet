using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using Digipost.Api.Client.Shared.Resources.Resource;
using Digipost.Signature.Api.Client.Core.Internal.Utilities;

namespace Digipost.Signature.Api.Client.Core.Tests.Utilities
{
    public static class CoreResponseUtility
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "Digipost.Signature.Api.Client.Core.Tests.Schema.Examples.Response");

        public static string GetBrokerNotAuthorizedError()
        {
            return GetXml("BrokerNotAuthorizedErrorResponse.xml").OuterXml;
        }

        public static string GetError()
        {
            return GetXml("ErrorResponse.xml").OuterXml;
        }

        public static Stream GetPades()
        {
            return new MemoryStream(GetContentBytes("Pades.pdf"));
        }

        public static string GetEmptyQueueContent()
        {
            return GetContent("EmptyQueueResponse.txt");
        }

        public static string GetTooManyRequestsResponse()
        {
            return GetContent("TooManyRequestsResponse.txt");
        }

        internal static XmlDocument GetXml(string kvittering)
        {
            return XmlUtility.ToXmlDocument(GetContent(kvittering));
        }

        private static byte[] GetContentBytes(string path)
        {
            return ResourceUtility.ReadAllBytes(path);
        }

        internal static string GetContent(string path)
        {
            return Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(path));
        }
    }
}