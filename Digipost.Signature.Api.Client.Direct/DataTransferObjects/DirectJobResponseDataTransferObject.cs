using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Direct.DataTransferObjects
{
    [XmlRoot(ElementName = "direct-signature-job-response", Namespace = "http://signering.posten.no/schema/v1")]
    public class DirectJobResponseDataTransferObject
    {
        [XmlElement("signature-job-id")]
        public string SignatureJobId { get; set; }

        [XmlElement("redirect-url")]
        public string RedirectUrl{ get; set; }

        [XmlElement("status-url")]
        public string StatusUrl { get; set; }
    }
}
