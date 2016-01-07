using System;
using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Core
{
    [Serializable]
    [XmlRoot(Namespace = "http://signering.digipost.no/schema/v1", IsNullable = false)]
    [XmlType(TypeName = "signer", Namespace = "http://signering.digipost.no/schema/v1/common")]
    public class Signer
    {
        public string PersonalIdentificationNumber { get;}

        public Signer(string personalIdentificationNumber)
        {
            PersonalIdentificationNumber = personalIdentificationNumber;
        }
    }
}
