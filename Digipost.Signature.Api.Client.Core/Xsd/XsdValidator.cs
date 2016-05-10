using Difi.Felles.Utility;
using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;

namespace Digipost.Signature.Api.Client.Core.Xsd
{
    internal class XsdValidator : XmlValidator
    {
        public XsdValidator()
        {
            var httpSigneringPostenNoSchemaV1 = "http://signering.posten.no/schema/v1";
            AddXsd(httpSigneringPostenNoSchemaV1, XsdFiles.GetCommon());
            AddXsd(httpSigneringPostenNoSchemaV1, XsdFiles.GetDirect());
            AddXsd(httpSigneringPostenNoSchemaV1, XsdFiles.GetPortal());
        }
    }
}