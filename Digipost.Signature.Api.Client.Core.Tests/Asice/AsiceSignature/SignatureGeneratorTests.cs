﻿using System.Linq;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Core.Xsd;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice.AsiceSignature
{
    [TestClass]
    public class SignatureGeneratorTests
    {
        [TestClass]
        public class ConstructorMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void InitializesWithDocumentManifestAndCertificate()
            {
                //Arrange
                var document = DomainUtility.GetDocument();
                var sender = DomainUtility.GetSender();
                var manifest = new DirectManifest(sender, document, DomainUtility.GetSigners(3));
                var x509Certificate2 = DomainUtility.GetTestCertificate();

                //Act
                var signatur = new SignatureGenerator(x509Certificate2, document, manifest);

                //Assert
                Assert.AreEqual(document, signatur.Attachables.ElementAt(0));
                Assert.AreEqual(manifest, signatur.Attachables.ElementAt(1));
                Assert.AreEqual(x509Certificate2, signatur.Certificate);
            }
        }

        [TestClass]
        public class FileNameMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticFileName()
            {
                //Arrange
                var signaturGenerator = GetSignaturGenerator();
                const string expectedFileName = "META-INF/signatures.xml";

                //Act

                //Assert
                Assert.AreEqual(expectedFileName, signaturGenerator.FileName);
            }
        }

        [TestClass]
        public class IdMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticId()
            {
                //Arrange
                var signaturGenerator = GetSignaturGenerator();
                const string expectedId = "Id_0";

                //Act

                //Assert
                Assert.AreEqual(expectedId, signaturGenerator.Id);
            }
        }

        [TestClass]
        public class XmlMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void GeneratesValidSignatureXml()
            {
                //Arrange
                var signatureGenerator = DomainUtility.GetSignature();
                var xml = signatureGenerator.Xml().InnerXml;
                var signatureValidator = new SignatureValidator();

                //Act
                var isValidSignatureXml = signatureValidator.ValiderDokumentMotXsd(xml);
                int signatureLength = xml.Length;

                //Assert
                Assert.IsTrue(isValidSignatureXml);
                Assert.IsTrue(signatureLength > 3200);                
            }

        }

        internal SignatureGenerator GetSignaturGenerator()
        {
            var document = DomainUtility.GetDocument();
            var sender = DomainUtility.GetSender();
            var manifest = new DirectManifest(sender, document, DomainUtility.GetSigners(3));
            var x509Certificate2 = DomainUtility.GetTestCertificate();
            var signaturGenerator = new SignatureGenerator(x509Certificate2, document, manifest);
            return signaturGenerator;
        }
    }
}