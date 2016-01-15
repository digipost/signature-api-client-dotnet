using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Difi.Felles.Utility.Security;
using Difi.Felles.Utility.Utilities;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Core.Exceptions;

namespace Digipost.Signature.Api.Client.Core.Asice.AsiceSignature
{
    internal class SignatureGenerator : IAsiceAttachable
    {
        public Document Document { get; }

        public Manifest Manifest { get; }

        public X509Certificate2 Certificate { get; }

        private XmlDocument _xml;
        private SignedXml _signatureNode;

        public SignatureGenerator(Document document, Manifest manifest, X509Certificate2 certificate)
        {
            Document = document;
            Manifest = manifest;
            Certificate = certificate;
        }

        public string FileName
        {
            get { return "META-INF/signatures.xml"; }
        }
        
        public byte[] Bytes
        {
            get
            {
                return Encoding.UTF8.GetBytes(Xml().OuterXml);
            }
        }

        public string MimeType
        {
            get { return "application/xml"; }
        }

        public string Id
        {
            get { return "Id_0"; }
        }

        public XmlDocument Xml()
        {
            if (_xml != null)
            {
                return _xml;
            }

            try
            {
                CreateXadesSignature();
            }
            catch (Exception e)
            {
                throw new XmlParseException("Kunne ikke bygge Xml for signatur.", e);
            }

            return _xml;
        }

        private void CreateXadesSignature()
        {
            _xml = CreateXadesSignatureElement();
            _signatureNode = CreateSignatureElement();

            AddReferences(Manifest, Document);
            AddKeyInfo();

            _signatureNode.ComputeSignature();

            AddSignatureToDocument();
        }

        private XmlDocument CreateXadesSignatureElement()
        {
            var signatureDocument = new XmlDocument { PreserveWhitespace = true };
            var xmlDeclaration = signatureDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            signatureDocument.AppendChild(signatureDocument.CreateElement("xades", "XAdESSignatures", NavneromUtility.UriEtsi121));
            signatureDocument.DocumentElement.SetAttribute("xmlns", NavneromUtility.UriEtsi132);

            //Todo: Legg til foerst
            signatureDocument.InsertBefore(xmlDeclaration, signatureDocument.DocumentElement);
            return signatureDocument;
        }

        private SignedXml CreateSignatureElement()
        {
            SignedXml signedXml = new SignedXmlWithAgnosticId(_xml, Certificate);
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
            signedXml.Signature.Id = "Signature";
            return signedXml;
        }

        private void AddReferences(params IAsiceAttachable[] asiceAttachables)
        {
            foreach (var item in asiceAttachables)
            {
                _signatureNode.AddReference(Sha256Reference(item));
            }

            _signatureNode.AddObject(
                new QualifyingPropertiesObject(
                    Certificate, 
                    target: "#Signature", 
                    references: asiceAttachables,
                    context: _xml.DocumentElement
                    )
                );

            _signatureNode.AddReference(SignedPropertiesReference());
        }

        private void AddKeyInfo()
        {
            var keyInfoX509Data = new KeyInfoX509Data(Certificate, X509IncludeOption.WholeChain);
            _signatureNode.KeyInfo.AddClause(keyInfoX509Data);
        }

        private static Sha256Reference SignedPropertiesReference()
        {
            var signedPropertiesReference = new Sha256Reference("#SignedProperties")
            {
                Type = "http://uri.etsi.org/01903#SignedProperties"
            };
            signedPropertiesReference.AddTransform(new XmlDsigC14NTransform(false));
            return signedPropertiesReference;
        }

        private Sha256Reference Sha256Reference(IAsiceAttachable document)
        {
            return new Sha256Reference(document.Bytes)
            {
                Uri = document.FileName,
                Id = document.Id
            };
        }

        private void AddSignatureToDocument()
        {
            var xml = _signatureNode.GetXml().InnerXml;
             _xml.DocumentElement.AppendChild(_xml.ImportNode(_signatureNode.GetXml(), deep: true));
        }
    }
}
