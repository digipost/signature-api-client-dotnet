using Difi.Felles.Utility;
using Digipost.Signature.Api.Client.Resources.Xsd;
using Digipost.Signature.Api.Client.Scripts.XsdToCode.Code;

namespace Digipost.Signature.Api.Client.Core.Internal.Xsd
{
    internal class XsdValidator : XmlValidator
    {
        public XsdValidator()
        {
            const string httpSigneringPostenNoSchemaV1 = "http://signering.posten.no/schema/v1";
            const string uriEtsi121 = "http://uri.etsi.org/2918/v1.2.1#";
            const string uriEtsi132 = "http://uri.etsi.org/01903/v1.3.2#";
            const string xmlDsig = "http://www.w3.org/2000/09/xmldsig#";

            AddXsd(httpSigneringPostenNoSchemaV1, XsdFiles.GetCommon());
            AddXsd(httpSigneringPostenNoSchemaV1, XsdFiles.GetDirect());
            AddXsd(httpSigneringPostenNoSchemaV1, XsdFiles.GetPortal());
            AddXsd(uriEtsi121, XsdResource.Signature.GetTsXsd());
            AddXsd(uriEtsi132, XsdResource.Signature.GetXadesXsd());
            AddXsd(xmlDsig, XsdResource.Signature.GetXmldsigCoreXsd());
        }
    }
}