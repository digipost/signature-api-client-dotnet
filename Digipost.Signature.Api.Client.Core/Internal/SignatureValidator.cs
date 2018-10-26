using System.IO;
using System.Reflection;
using System.Xml;
using Digipost.Api.Client.Shared.Resources.Resource;
using Digipost.Api.Client.Shared.Xml;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal class SignatureValidator : XmlValidator
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "Digipost.Signature.Api.Client.Core.Internal.Xsd.Thirdparty");

        public SignatureValidator()
        {
            AddXsd("http://uri.etsi.org/01903/v1.3.2#", HentRessurs("XAdES.xsd"));
            AddXsd("http://www.w3.org/2000/09/xmldsig#", HentRessurs("xmldsig-core-schema.xsd"));
            AddXsd("http://uri.etsi.org/2918/v1.2.1#", HentRessurs("ts_102918v010201.xsd"));
        }

        private XmlReader HentRessurs(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}
