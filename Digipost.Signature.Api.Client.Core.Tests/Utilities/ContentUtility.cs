using System.Text;
using ApiClientShared;

namespace Digipost.Signature.Api.Client.Core.Tests.Utilities
{
    internal static class ContentUtility
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Signature.Api.Client.Core.Tests.Resources.Body");

        internal static string GetDirectSignatureJobRequestBody()
        {
            return GetContent("DirectSignatureJobRequest.xml");
        }

        internal static string GetDirectSignatureJobRequestBodyInvalid()
        {
            return GetContent("DirectSignatureJobRequestInvalid.xml");
        }

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
            return Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(true, path));
        }
    }
}