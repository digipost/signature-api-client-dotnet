using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Difi.Felles.Utility.Exceptions;

namespace Difi.Felles.Utility.Security
{
    /// <summary>
    ///     Enhances the core SignedXml provider with namespace agnostic query for Id elements.
    /// </summary>
    /// <remarks>
    ///     From:
    ///     http://stackoverflow.com/questions/5099156/malformed-reference-element-when-adding-a-reference-based-on-an-id-attribute-w
    /// </remarks>
    public sealed class SignedXmlWithAgnosticId : SignedXml
    {
        private const int PROV_RSA_AES = 24; // CryptoApi provider type for an RSA provider supporting sha-256 digital signatures

        private readonly List<AsymmetricAlgorithm> _publicKeys = new List<AsymmetricAlgorithm>();

        private readonly XmlDocument _xmlDokument;

        private IEnumerator<AsymmetricAlgorithm> _publicKeyListEnumerator;

        public SignedXmlWithAgnosticId(XmlDocument xmlDocument)
            : base(xmlDocument)
        {
            _xmlDokument = xmlDocument;
        }

        /// <summary>
        ///     Sets SHA256 as signaure method and XmlDsigExcC14NTransformUrl as canonicalization method
        /// </summary>
        /// <param name="xmlDocument">The document containing the references to be signed.</param>
        /// <param name="certificate">The certificate containing the private key used for signing.</param>
        /// <param name="inclusiveNamespacesPrefixList">
        ///     An optional list of namespaces to be set as the canonicalization namespace
        ///     prefix list.
        /// </param>
        public SignedXmlWithAgnosticId(XmlDocument xmlDocument, X509Certificate2 certificate, string inclusiveNamespacesPrefixList = null)
            : base(xmlDocument)
        {
            const string signatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

            // Adds signature method to crypto api
            if (CryptoConfig.CreateFromName(signatureMethod) == null)
                CryptoConfig.AddAlgorithm(typeof(RsaPkCs1Sha256SignatureDescription), signatureMethod);

            // Makes sure the signingkey is using Microsoft Enhanced RSA and AES Cryptographic Provider which enables SHA256
            if (!certificate.HasPrivateKey)
                throw new SecurityException(string.Format("Angitt sertifikat med fingeravtrykk {0} inneholder ikke en privatnøkkel. Dette er påkrevet for å signere xml dokumenter.", certificate.Thumbprint));

            var targetKey = certificate.PrivateKey as RSACryptoServiceProvider;
            if (targetKey == null)
                throw new SecurityException(string.Format("Privatnøkkelen i sertifikatet med fingeravtrykk {0} er ikke en gyldig RSA asymetrisk nøkkel.", certificate.Thumbprint));

            if (targetKey.CspKeyContainerInfo.ProviderType == PROV_RSA_AES)
                SigningKey = targetKey;
            else
            {
                SigningKey = new RSACryptoServiceProvider();
                try
                {
                    SigningKey.FromXmlString(certificate.PrivateKey.ToXmlString(true));
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Angitt sertifikat med fingeravtrykk {0} kan ikke eksporteres. Det er nødvendig når sertifikatet ikke er opprettet med 'Microsoft Enhanced RSA and AES Cryptographic Provider' som CryptoAPI provider name (-sp parameter i makecert.exe eller -csp parameter i openssl).", certificate.Thumbprint), e);
                }
            }

            SignedInfo.SignatureMethod = signatureMethod;
            SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
            if (inclusiveNamespacesPrefixList != null)
                ((XmlDsigExcC14NTransform)SignedInfo.CanonicalizationMethodObject).InclusiveNamespacesPrefixList = inclusiveNamespacesPrefixList;

            _xmlDokument = xmlDocument;
        }

        public AsymmetricAlgorithm PublicKey { get; private set; }

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            // Attemt to find id node using standard methods. If not found, attempt using namespace agnostic method.
            var idElem = base.GetIdElement(doc, id) ?? FindIdElement(doc, id);

            // Check to se if id element is within the signatures object node. This is used by ESIs Xml Advanced Electronic Signatures (Xades)
            if (idElem == null)
            {
                if ((Signature != null) && (Signature.ObjectList != null))
                {
                    foreach (DataObject dataObject in Signature.ObjectList)
                    {
                        if ((dataObject.Data != null) && (dataObject.Data.Count > 0))
                        {
                            foreach (XmlNode dataNode in dataObject.Data)
                            {
                                idElem = FindIdElement(dataNode, id);
                                if (idElem != null)
                                {
                                    return idElem;
                                }
                            }
                        }
                    }
                }
            }

            return idElem;
        }

        protected override AsymmetricAlgorithm GetPublicKey()
        {
            var publicKey = base.GetPublicKey() ?? GetNextKey();
            return PublicKey = publicKey;
        }

        private XmlElement FindIdElement(XmlNode node, string idValue)
        {
            XmlElement result = null;
            foreach (var s in new[] { "Id", "ID", "id" })
            {
                result = node.SelectSingleNode(string.Format("//*[@*[local-name() = '{0}'] = '{1}']", s, idValue)) as XmlElement;
                if (result != null)
                    break;
            }

            return result;
        }

        private AsymmetricAlgorithm GetNextKey()
        {
            AsymmetricAlgorithm publicKey = null;

            if (KeyInfo == null)
            {
                throw new CryptographicException("Kryptografi_Xml_Keyinfo nødvendig");
            }

            if (_publicKeyListEnumerator == null)
            {
                GetPublicKeysAndSetEnumerator();
            }

            while ((_publicKeyListEnumerator != null) && _publicKeyListEnumerator.MoveNext())
            {
                publicKey = _publicKeyListEnumerator.Current;
            }

            return publicKey;
        }

        private void GetPublicKeysAndSetEnumerator()
        {
            var keyInfoXml = KeyInfo.GetXml();
            if (!keyInfoXml.IsEmpty)
            {
                var keyInfoNamespaceMananger = GetKeyInfoNamespaceMananger(keyInfoXml);
                var securityTokenReference = SecurityTokenReference(keyInfoXml, keyInfoNamespaceMananger);

                if (securityTokenReference != null)
                {
                    var binarySecurityTokenSertifikat = GetBinarySecurityToken(securityTokenReference);

                    _publicKeys.Add(binarySecurityTokenSertifikat.PublicKey.Key);
                    _publicKeyListEnumerator = _publicKeys.GetEnumerator();
                }
            }
        }

        private static XmlNamespaceManager GetKeyInfoNamespaceMananger(XmlElement keyInfoXml)
        {
            var mgr = new XmlNamespaceManager(keyInfoXml.OwnerDocument.NameTable);
            mgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            mgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

            return mgr;
        }

        private static XmlNode SecurityTokenReference(XmlElement keyInfoXml, XmlNamespaceManager keyInfoNamespaceMananger)
        {
            return keyInfoXml.SelectSingleNode("./wsse:SecurityTokenReference/wsse:Reference", keyInfoNamespaceMananger);
        }

        private X509Certificate2 GetBinarySecurityToken(XmlNode securityTokenReference)
        {
            var securityTokenReferenceUri = GetSecurityTokenReferenceUri(securityTokenReference);
            X509Certificate2 publicCertificate = null;

            var keyElement = FindIdElement(_xmlDokument, securityTokenReferenceUri);
            if ((keyElement != null) && !string.IsNullOrEmpty(keyElement.InnerText))
            {
                publicCertificate = new X509Certificate2(Convert.FromBase64String(keyElement.InnerText));
            }

            return publicCertificate;
        }

        private string GetSecurityTokenReferenceUri(XmlNode reference)
        {
            var uriRefereanseAttributt = reference.Attributes["URI"];

            if (uriRefereanseAttributt == null)
            {
                throw new SecurityException("Klarte ikke finne SecurityTokenReferenceUri.");
            }

            var referenceUriValue = uriRefereanseAttributt.Value;
            if (referenceUriValue.StartsWith("#"))
            {
                referenceUriValue = referenceUriValue.Substring(1);
            }

            return referenceUriValue;
        }
    }
}