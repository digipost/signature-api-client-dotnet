using System.Text;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility.Utilities;

namespace Digipost.Signature.Api.Client.Portal.Tests.Utilities
{
    public static class ResponseUtility
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Signature.Api.Client.Portal.Tests.Schema.Examples.Response");

        public static string GetEmptyQueueContent()
        {
            return GetContent("EmptyQueueResponse.txt");
        }

        public static string GetJobStatusChangeResponse()
        {
            return GetXml("JobStatusChangeResponse.xml").OuterXml;
        }

        public static string GetTooManyRequestsResponse()
        {
            return GetContent("TooManyRequestsResponse.txt");
        }

        public static string GetError()
        {
            return GetXml("ErrorResponse.xml").OuterXml;
        }

        public static string GetBrokerNotAuthorizedError()
        {
            return GetXml("BrokerNotAuthorizedErrorResponse.xml").OuterXml;
        }

        internal static XmlDocument GetXml(string kvittering)
        {
            return XmlUtility.TilXmlDokument(GetContent(kvittering));
        }

        internal static string GetContent(string path)
        {
            return Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(true, path));
        }
    }
}
