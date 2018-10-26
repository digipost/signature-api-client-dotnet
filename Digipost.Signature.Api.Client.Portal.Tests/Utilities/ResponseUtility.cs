using System.Reflection;
using System.Text;
using System.Xml;
using Digipost.Api.Client.Shared.Resources.Resource;
using Digipost.Signature.Api.Client.Core.Internal.Utilities;

namespace Digipost.Signature.Api.Client.Portal.Tests.Utilities
{
    public static class ResponseUtility
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "Digipost.Signature.Api.Client.Portal.Tests.Schema.Examples.Response");

        public static string GetJobStatusChangeResponse()
        {
            return GetXml("JobStatusChangeResponse.xml").OuterXml;
        }

        internal static XmlDocument GetXml(string kvittering)
        {
            return XmlUtility.ToXmlDocument(GetContent(kvittering));
        }

        internal static string GetContent(string path)
        {
            return Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(path));
        }
    }
}
