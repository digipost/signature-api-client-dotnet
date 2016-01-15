using System;
using System.Xml;
using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Direct.DataTransferObjects
{
    public class ExitUrlsDataTranferObject
    {
        [XmlElement("completion-url")]
        public Uri CompletionUrl { get; set; }

        [XmlElement("completion-url")]
        public Uri CancellationUrl { get; set; }

        [XmlElement("completion-url")]
        public Uri ErrorUrl { get; set; }
    }
}
