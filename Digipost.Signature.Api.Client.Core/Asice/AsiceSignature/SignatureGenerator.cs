using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Difi.Felles.Utility.Security;
using Difi.Felles.Utility.Utilities;
using Difi.SikkerDigitalPost.Klient.Security;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Core.Exceptions;

namespace Digipost.Signature.Api.Client.Core.Asice.AsiceSignature
{
    internal class Signatur : IAsiceAttachable
    {
        private readonly Document _document;
        //private readonly Forsendelse _forsendelse;
        private readonly Manifest _manifest;
        private readonly X509Certificate2 _sertifikat;
        private XmlDocument _xml;

        public Signatur(Document document, Manifest manifest, X509Certificate2 sertifikat)
        {
            _document = document;
            _manifest = manifest;
            _sertifikat = sertifikat;
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

                IEnumerable<IAsiceAttachable> referanser = Referanser(_document, _manifest);
                OpprettReferanser(signaturnode, referanser);

                var keyInfoX509Data = new KeyInfoX509Data(_sertifikat, X509IncludeOption.WholeChain);
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
            SignedXml signedXml = new SignedXmlWithAgnosticId(_xml, _sertifikat);
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
                    _sertifikat, "#Signature", referanser.ToArray(), _xml.DocumentElement)
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
