﻿using System.Xml;

namespace Digipost.Signature.Api.Client.Resources.Xml
{
    internal class XmlUtility
    {
        public static XmlDocument ToXmlDocument(string kvittering)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(kvittering);

            return xmlDocument;
        }
    }
}
