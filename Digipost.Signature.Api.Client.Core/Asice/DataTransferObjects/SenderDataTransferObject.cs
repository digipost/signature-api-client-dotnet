using System;
using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects
{
    [Serializable]
    [XmlType(TypeName = "sender")]
    public class SenderDataTransferObject
    {
        [XmlElement("organization")]
        public string Organization { get; set; }
    }
}