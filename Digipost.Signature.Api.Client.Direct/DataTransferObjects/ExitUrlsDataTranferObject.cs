using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Direct.DataTransferObjects
{
    public class ExitUrlsDataTranferObject
    {
        [XmlElement("completion-url")]
        public string CompletionUrl { get; set; }

        [XmlElement("cancellation-url")]
        public string CancellationUrl { get; set; }

        [XmlElement("error-url")]
        public string ErrorUrl { get; set; }
    }
}
