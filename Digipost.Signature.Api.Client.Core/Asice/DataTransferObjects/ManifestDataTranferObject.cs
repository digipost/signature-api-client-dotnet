using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects
{
    [Serializable]
    [XmlRoot(Namespace = "http://signering.posten.no/schema/v1", IsNullable = false)]
    [XmlType(TypeName = "manifest")]
    public class ManifestDataTranferObject
    {
        [XmlArray("signers")]
        [XmlArrayItem("signer")]
        public List<SignerDataTranferObject> SignersDataTransferObjects = new List<SignerDataTranferObject>();

        [XmlElement("sender")]
        public SenderDataTransferObject SenderDataTransferObject { get; set; }

        [XmlElement("primary-document")]
        public DocumentDataTransferObject DocumentDataTransferObject { get; set; }
    }
}
