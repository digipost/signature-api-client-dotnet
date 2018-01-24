using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Digipost.Signature.Api.Client.Core.Exceptions;

namespace Digipost.Signature.Api.Client.Core.Internal.Asice.AsiceSignature
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
        private const int RsaSha256DigitalSignaturesCryptoApiProvider = 24;
        private const string CanocalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
        private new const string SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
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
            AddSignatureMethodToCryptoApi(SignatureMethod);
            AssertHasPrivateKeyOrThrow(certificate);

            SetSigningKey(certificate);
            SetSignedInfo(inclusiveNamespacesPrefixList);

            _xmlDokument = xmlDocument;
        }

        private static void AddSignatureMethodToCryptoApi(string signatureMethod)
        {
            if (CryptoConfig.CreateFromName(signatureMethod) == null)
                CryptoConfig.AddAlgorithm(typeof(RsaPkCs1Sha256SignatureDescription), signatureMethod);
        }

        private static void AssertHasPrivateKeyOrThrow(X509Certificate2 certificate)
        {
            if (!certificate.HasPrivateKey)
                throw new SecurityException($"Specified certificate with fingerprint {certificate.Thumbprint} does not contain a private key, which is mandatory when signing XML documents.");
        }

        private void SetSigningKey(X509Certificate2 certificate)
        {
            var targetKey = ExtractValidPrivateKeyOrThrow(certificate);

            /*
            Todo: 
            The commented lines here are removed because they seem to not be needed when using RSA directly
            in ExtractValidPrivateKeyOrThrow. This may be because we are not fetching a certificate that may
            not be exportable, since we are only loading from file.
            */

//            if (targetKey.CspKeyContainerInfo.ProviderType == RsaSha256DigitalSignaturesCryptoApiProvider)
            SigningKey = targetKey;
//            else
//            {
//                SigningKey = new RSACryptoServiceProvider();
//                try
//                {
//                    SigningKey.FromXmlString(certificate.PrivateKey.ToXmlString(true));
//                }
//                catch (Exception e)
//                {
//                    throw new Exception($"Specified certificate with fingerprint {certificate.Thumbprint} cannot be exported. This is required when the certificate isn't created with 'Microsoft Enhanced RSA and AES Cryptographic Provider' as CryptoAPI provider name (-sp parameter i makecert.exe or -csp parameter i openssl).", e);
//                }
            //}
        }

        private static RSA ExtractValidPrivateKeyOrThrow(X509Certificate2 certificate)
        {
            var targetKey = certificate.GetRSAPrivateKey();
            if (targetKey == null)

                throw new SecurityException($"Specified certificate with fingerprint {certificate.Thumbprint} is not a valid RSA asymetric key.");

            return targetKey;
        }

        private void SetSignedInfo(string inclusiveNamespacesPrefixList)
        {
            SignedInfo.SignatureMethod = SignatureMethod;
            SignedInfo.CanonicalizationMethod = CanocalizationMethod;
            if (inclusiveNamespacesPrefixList != null)
                ((XmlDsigExcC14NTransform) SignedInfo.CanonicalizationMethodObject).InclusiveNamespacesPrefixList = inclusiveNamespacesPrefixList;
        }

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            // Attemt to find id node using standard methods. If not found, attempt using namespace agnostic method.
            var idElem = base.GetIdElement(doc, id) ?? FindIdElement(doc, id);

            // Check to se if id element is within the signatures object node. This is used by ESIs Xml Advanced Electronic Signatures (Xades)
            if (idElem == null && Signature?.ObjectList != null)
            {
                foreach (DataObject dataObject in Signature.ObjectList)
                {
                    if (dataObject.Data?.Count > 0)
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

            return idElem;
        }

        protected override AsymmetricAlgorithm GetPublicKey()
        {
            return base.GetPublicKey() ?? GetNextKey();
        }

        private static XmlElement FindIdElement(XmlNode node, string idValue)
        {
            XmlElement result = null;
            foreach (var s in new[] {"Id", "ID", "id"})
            {
                result = node.SelectSingleNode($"//*[@*[local-name() = '{s}'] = '{idValue}']") as XmlElement;

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
                throw new CryptographicException("Cryptography Xml Keyinfo is necessary to retrieve the public key.");
            }

            if (_publicKeyListEnumerator == null)
            {
                GetPublicKeysAndSetEnumerator();
            }

            while (_publicKeyListEnumerator != null && _publicKeyListEnumerator.MoveNext())
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

        private static XmlNode SecurityTokenReference(XmlNode keyInfoXml, XmlNamespaceManager keyInfoNamespaceMananger)
        {
            return keyInfoXml.SelectSingleNode("./wsse:SecurityTokenReference/wsse:Reference", keyInfoNamespaceMananger);
        }

        private X509Certificate2 GetBinarySecurityToken(XmlNode securityTokenReference)
        {
            var securityTokenReferenceUri = GetSecurityTokenReferenceUri(securityTokenReference);
            X509Certificate2 publicCertificate = null;

            var keyElement = FindIdElement(_xmlDokument, securityTokenReferenceUri);
            if (!string.IsNullOrEmpty(keyElement?.InnerText))
            {
                publicCertificate = new X509Certificate2(Convert.FromBase64String(keyElement?.InnerText));
            }

            return publicCertificate;
        }

        private static string GetSecurityTokenReferenceUri(XmlNode reference)
        {
            var uriReferenceAttribute = reference.Attributes?["URI"];

            if (uriReferenceAttribute == null)
            {
                throw new SecurityException("Klarte ikke finne SecurityTokenReferenceUri.");
            }

            var referenceUriValue = uriReferenceAttribute.Value;
            if (referenceUriValue.StartsWith("#"))
            {
                referenceUriValue = referenceUriValue.Substring(1);
            }

            return referenceUriValue;
        }
    }
}