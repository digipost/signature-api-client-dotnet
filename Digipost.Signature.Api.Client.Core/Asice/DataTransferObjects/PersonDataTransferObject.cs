using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Core.Asice.AsiceManifest
{
    public class PersonDataTransferObject
    {
        [XmlElement("personal-identification-number")]
        public string PersonalIdentificationNumber { get; set; }
    }
}