//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Security.Cryptography.X509Certificates;
//using System.Security.Cryptography.Xml;
//using System.Text;
//using System.Xml;
//using Difi.Felles.Utility.Security;
//using Difi.Felles.Utility.Utilities;
//using Digipost.Signature.Api.Client.Asice.Manifest;
//using Sha256Reference = Difi.SikkerDigitalPost.Klient.Domene.Sha256Reference;

//namespace Digipost.Signature.Api.Client.Asice.Signature
//{
//    internal class Signatur : IAsiceAttachable
//    {
//        private readonly Forsendelse _forsendelse;
//        private readonly Manifest _manifest;
//        private readonly X509Certificate2 _sertifikat;
//        private XmlDocument _xml;

//        public Signatur(Forsendelse forsendelse, Manifest manifest, X509Certificate2 sertifikat)
//        {
//            _forsendelse = forsendelse;
//            _manifest = manifest;
//            _sertifikat = sertifikat;
//        }

//        public string FileName
//        {
//            get { return "META-INF/signatures.xml"; }
//        }

//        public byte[] Bytes
//        {
//            get
//            {
//                return Encoding.UTF8.GetBytes(Xml().OuterXml);
//            }
//        }

//        public string MimeType
//        {
//            get { return "application/xml"; }
//        }

//        public string Id
//        {
//            get { return "Id_0"; }
//        }

//        public XmlDocument Xml()
//        {
//            if (_xml != null)
//            {
//                return _xml;
//            }

//            try
//            {
//                _xml = OpprettXmlDokument();

//                var signaturnode = Signaturnode();

//                IEnumerable<IAsiceAttachable> referanser = Referanser(_forsendelse.Dokumentpakke.Hoveddokument,
//                    _forsendelse.Dokumentpakke.Vedlegg, _manifest);
//                OpprettReferanser(signaturnode, referanser);

//                var keyInfoX509Data = new KeyInfoX509Data(_sertifikat, X509IncludeOption.WholeChain);
//                signaturnode.KeyInfo.AddClause(keyInfoX509Data);
//                signaturnode.ComputeSignature();

//                _xml.DocumentElement.AppendChild(_xml.ImportNode(signaturnode.GetXml(), deep: true));
//            }
//            catch (Exception e)
//            {
//                throw new XmlParseException("Kunne ikke bygge Xml for signatur.", e);
//            }

//            return _xml;
//        }

//        private XmlDocument OpprettXmlDokument()
//        {
//            var signaturXml = new XmlDocument { PreserveWhitespace = true };
//            var xmlDeclaration = signaturXml.CreateXmlDeclaration("1.0", "UTF-8", null);
//            signaturXml.AppendChild(signaturXml.CreateElement("xades", "XAdESSignatures", NavneromUtility.UriEtsi121));
//            signaturXml.DocumentElement.SetAttribute("xmlns:ns11", NavneromUtility.UriEtsi132);

//            signaturXml.InsertBefore(xmlDeclaration, signaturXml.DocumentElement);
//            return signaturXml;
//        }

//        private SignedXml Signaturnode()
//        {
//            SignedXml signedXml = new SignedXmlWithAgnosticId(_xml, _sertifikat);
//            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
//            signedXml.Signature.Id = "Signature";
//            return signedXml;
//        }

//        private static IEnumerable<IAsiceAttachable> Referanser(Dokument hoveddokument, IEnumerable<IAsiceAttachable> vedlegg, ManifestDataTranferObject manifest)
//        {
//            var referanser = new List<IAsiceAttachable>();
//            referanser.Add(hoveddokument);
//            referanser.AddRange(vedlegg);
//            referanser.Add(manifest);
//            return referanser;
//        }

//        private static Sha256Reference SignedPropertiesReferanse()
//        {
//            var signedPropertiesReference = new Sha256Reference("#SignedProperties")
//            {
//                Type = "http://uri.etsi.org/01903#SignedProperties"
//            };
//            signedPropertiesReference.AddTransform(new XmlDsigC14NTransform(false));
//            return signedPropertiesReference;
//        }

//        private void OpprettReferanser(SignedXml signaturnode, IEnumerable<IAsiceVedlegg> referanser)
//        {
//            foreach (var item in referanser)
//            {
//                signaturnode.AddReference(Sha256Referanse(item));
//            }

//            signaturnode.AddObject(
//                new QualifyingPropertiesObject(
//                    _sertifikat, "#Signature", referanser.ToArray(), _xml.DocumentElement)
//                    );

//            signaturnode.AddReference(SignedPropertiesReferanse());
//        }

//        private Sha256Reference Sha256Referanse(IAsiceVedlegg dokument)
//        {
//            return new Sha256Reference(dokument.Bytes)
//            {
//                Uri = dokument.Filnavn,
//                Id = dokument.Id
//            };
//        }
//    }
//}
