using System.Xml;
using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Asice.Manifest
{
    public class PersonDataTransferObject
    {
        [XmlElement("personal-identification-number")]
        public string PersonalIdentificationNumber { get; set; }
    }
}