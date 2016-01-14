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
    internal class Signature : IAsiceAttachable
    {
        public Document Document { get; }

        public Manifest Manifest { get; }

        public X509Certificate2 Certificate { get; }

        private XmlDocument _xml;

        public Signature(Document document, Manifest manifest, X509Certificate2 certificate)
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

        public FileType FileType { get; }

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
                _xml = OpprettXmlDokument();

                var signaturnode = Signaturnode();

                IEnumerable<IAsiceAttachable> referanser = Referanser(Document, Manifest);
                OpprettReferanser(signaturnode, referanser);

                var keyInfoX509Data = new KeyInfoX509Data(Certificate, X509IncludeOption.WholeChain);
                signaturnode.KeyInfo.AddClause(keyInfoX509Data);
                signaturnode.ComputeSignature();

                _xml.DocumentElement.AppendChild(_xml.ImportNode(signaturnode.GetXml(), deep: true));
            }
            catch (Exception e)
            {
                throw new XmlParseException("Kunne ikke bygge Xml for signatur.", e);
            }

            return _xml;
        }

        private XmlDocument OpprettXmlDokument()
        {
            var signaturXml = new XmlDocument { PreserveWhitespace = true };
            var xmlDeclaration = signaturXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            signaturXml.AppendChild(signaturXml.CreateElement("xades", "XAdESSignatures", NavneromUtility.UriEtsi121));
            signaturXml.DocumentElement.SetAttribute("xmlns:ns11", NavneromUtility.UriEtsi132);

            signaturXml.InsertBefore(xmlDeclaration, signaturXml.DocumentElement);
            return signaturXml;
        }

        private SignedXml Signaturnode()
        {
            SignedXml signedXml = new SignedXmlWithAgnosticId(_xml, Certificate);
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
            signedXml.Signature.Id = "Signature";
            return signedXml;
        }

        private static IEnumerable<IAsiceAttachable> Referanser(Document document, Manifest manifest)
        {
            var referanser = new List<IAsiceAttachable>();
            referanser.Add(document);
            referanser.Add(manifest);
            return referanser;
        }

        private static Sha256Reference SignedPropertiesReferanse()
        {
            var signedPropertiesReference = new Sha256Reference("#SignedProperties")
            {
                Type = "http://uri.etsi.org/01903#SignedProperties"
            };
            signedPropertiesReference.AddTransform(new XmlDsigC14NTransform(false));
            return signedPropertiesReference;
        }

        private void OpprettReferanser(SignedXml signaturnode, IEnumerable<IAsiceAttachable> referanser)
        {
            foreach (var item in referanser)
            {
                signaturnode.AddReference(Sha256Referanse(item));
            }

            signaturnode.AddObject(
                new QualifyingPropertiesObject(
                    Certificate, "#Signature", referanser.ToArray(), _xml.DocumentElement)
                    );

            signaturnode.AddReference(SignedPropertiesReferanse());
        }

        private Sha256Reference Sha256Referanse(IAsiceAttachable document)
        {
            return new Sha256Reference(document.Bytes)
            {
                Uri = document.FileName,
                Id = document.Id
            };
        }
    }
}
