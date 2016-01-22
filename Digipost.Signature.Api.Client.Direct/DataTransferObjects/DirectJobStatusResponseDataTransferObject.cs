using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Direct.DataTransferObjects
{
    [XmlRoot(ElementName = "direct-signature-job-status-response", Namespace = "http://signering.posten.no/schema/v1")]
    public class DirectJobStatusResponseDataTransferObject
    {
        [XmlElement("signature-job-id")]
        public string JobId { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("confirmation-url")]
        public string ComfirmationUrl { get; set; }

        [XmlElement("xades-url")]
        public string XadesUrl { get; set; }

        [XmlElement("pades-url")]
        public string PadesUrl { get; set; }
    }
}
