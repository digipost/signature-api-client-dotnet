﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects
{
    [Serializable]
    [XmlRoot(Namespace = "http://signering.digipost.no/schema/v1/signature-job", IsNullable = false)]
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
