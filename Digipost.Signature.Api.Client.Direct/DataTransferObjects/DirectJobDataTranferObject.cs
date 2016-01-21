using System;
using System.Xml.Serialization;
using Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Direct.DataTransferObjects
{
    [Serializable]
    [XmlRoot(Namespace = "http://signering.posten.no/schema/v1", IsNullable = false)]
    [XmlType(TypeName = "direct-signature-job-request")]
    public class DirectJobDataTranferObject
    {
        [XmlElement("reference")]
        public string Reference { get; set; }

        [XmlElement("signer")]
        public SignerDataTranferObject SignerDataTranferObject { get; set; }

        [XmlElement("sender")]
        public SenderDataTransferObject SenderDataTransferObject { get; set; }

        [XmlElement("exit-urls")]
        public ExitUrlsDataTranferObject ExitUrlsDataTranferObject { get; set; }
    }
}
