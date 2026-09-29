using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Digipost.Signature.Api.Client.Direct.Tests.Smoke
{
    /// <summary>
    ///     Not an assertion-driven test: prints exactly what SignatureGenerator embeds in the XAdES
    ///     &lt;KeyInfo&gt; for the certificate currently configured via CertificateReader (the same
    ///     one the smoke tests use), so the embedded chain can be eyeballed against what's expected -
    ///     see docs/adr/0002-midp-token-endpoint-ordinary-tls-validation.md for the equivalent mIdP-side
    ///     certificate concerns.
    /// </summary>
    public class SignatureCertificateChainDiagnostics
    {
        private readonly ITestOutputHelper _output;

        public SignatureCertificateChainDiagnostics(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Prints_certificates_embedded_in_the_xades_signature()
        {
            var certificate = CertificateReader.ReadCertificate();
            _output.WriteLine($"Signing certificate loaded from CertificateReader: {certificate.Subject}");
            _output.WriteLine($"  Issuer:     {certificate.Issuer}");
            _output.WriteLine($"  Thumbprint: {certificate.Thumbprint}");
            _output.WriteLine("");

            using (var chain = new X509Chain())
            {
                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                var chainBuilt = chain.Build(certificate);

                _output.WriteLine($"X509Chain.Build() returned: {chainBuilt}");
                _output.WriteLine($"Chain elements found: {chain.ChainElements.Count}");
                foreach (var element in chain.ChainElements)
                {
                    _output.WriteLine($"  - {element.Certificate.Subject}");
                    foreach (var status in element.ChainElementStatus)
                    {
                        _output.WriteLine($"      Status: {status.Status} - {status.StatusInformation.Trim()}");
                    }
                }

                foreach (var status in chain.ChainStatus)
                {
                    _output.WriteLine($"Overall chain status: {status.Status} - {status.StatusInformation.Trim()}");
                }

                _output.WriteLine("");
            }

            var documents = DomainUtility.GetSingleDirectDocument();
            var sender = CoreDomainUtility.GetSender();
            var manifest = new Manifest("Job title", sender, documents, DomainUtility.GetSigner());

            var signatureGenerator = new SignatureGenerator(certificate, documents, manifest);
            var xml = signatureGenerator.Xml();

            var namespaceManager = new XmlNamespaceManager(xml.NameTable);
            namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

            var certificateNodes = xml.SelectNodes("//ds:X509Data/ds:X509Certificate", namespaceManager);
            Assert.NotNull(certificateNodes);
            Assert.True(certificateNodes.Count > 0, "No X509Certificate elements found in the generated <KeyInfo> - AddKeyInfo/X509IncludeOption produced nothing.");

            _output.WriteLine($"Embedded {certificateNodes.Count} certificate(s) in <ds:KeyInfo><ds:X509Data>:");

            foreach (XmlNode node in certificateNodes)
            {
                var der = Convert.FromBase64String(node.InnerText);
                using var embeddedCertificate = new X509Certificate2(der);

                var isSelfSigned = embeddedCertificate.Subject == embeddedCertificate.Issuer;

                _output.WriteLine("");
                _output.WriteLine($"  Subject:     {embeddedCertificate.Subject}");
                _output.WriteLine($"  Issuer:      {embeddedCertificate.Issuer}");
                _output.WriteLine($"  Thumbprint:  {embeddedCertificate.Thumbprint}");
                _output.WriteLine($"  Self-signed: {isSelfSigned}");
            }
        }
    }
}
