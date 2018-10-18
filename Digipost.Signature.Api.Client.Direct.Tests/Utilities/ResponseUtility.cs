using System.Reflection;
using System.Text;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Digipost.Signature.Api.Client.Direct.Tests.Utilities
{
    internal static class ResponseUtility
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "Digipost.Signature.Api.Client.Direct.Tests.Schema.Examples.Response");

        internal static string GetCreateResponse()
        {
            return GetContent("CreateResponse.xml");
        }

        internal static string GetStatusResponse()
        {
            return GetContent("StatusResponse.xml");
        }

        internal static string GetContent(string path)
        {
            return Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(path));
        }
    }
}
