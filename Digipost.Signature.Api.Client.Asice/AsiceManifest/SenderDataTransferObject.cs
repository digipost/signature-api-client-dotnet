using System;
using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Asice.AsiceManifest
{
    [Serializable]
    [XmlType(TypeName = "sender")]
    public class SenderDataTransferObject
    {
        [XmlElement("organization")]
        public string Organization { get; set; }
    }
}