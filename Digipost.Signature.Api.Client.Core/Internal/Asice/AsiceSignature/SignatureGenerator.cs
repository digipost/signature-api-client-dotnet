using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal.Utilities.Digipost.Signature.Api.Client.Core.Internal.Utilities;

namespace Digipost.Signature.Api.Client.Core.Internal.Asice.AsiceSignature
{
    internal class SignatureGenerator : IAsiceAttachable
    {
        private SignedXml _signatureNode;
        private XmlDocument _xml;

        public SignatureGenerator(X509Certificate2 certificate, IEnumerable<IAsiceAttachable> documents, IAsiceAttachable manifest)
        {
            Certificate = certificate;
            Attachables = documents.Append(manifest).ToArray();
        }

        public X509Certificate2 Certificate { get; }

        public IAsiceAttachable[] Attachables { get; }

        public string FileName => "META-INF/signatures.xml";

        public byte[] Bytes => Encoding.UTF8.GetBytes(Xml().OuterXml);

        public string MimeType => "application/xml";

        public string Id => "Id_0";

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

            AddReferences();
            AddKeyInfo();

            _signatureNode.ComputeSignature();

            AddSignatureToDocument();
        }

        private XmlDocument CreateXadesSignatureElement()
        {
            var signatureDocument = new XmlDocument {PreserveWhitespace = true};
            var xmlDeclaration = signatureDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            signatureDocument.AppendChild(signatureDocument.CreateElement("xades", "XAdESSignatures", NamespaceUtility.UriEtsi121));
            signatureDocument.DocumentElement.SetAttribute("xmlns", NamespaceUtility.UriEtsi132);

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

        private void AddReferences()
        {
            foreach (var item in Attachables)
            {
                _signatureNode.AddReference(Sha256Reference(item));
            }

            _signatureNode.AddObject(
                new QualifyingPropertiesObject(
                    Certificate, "#Signature", Attachables, _xml.DocumentElement
                )
            );

            _signatureNode.AddReference(SignedPropertiesReference());
        }

        private void AddKeyInfo()
        {
            var keyInfoX509Data = new KeyInfoX509Data(Certificate, X509IncludeOption.EndCertOnly);
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
            _xml.DocumentElement.AppendChild(_xml.ImportNode(_signatureNode.GetXml(), true));
        }
    }
}
