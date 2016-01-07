using System;
using System.Collections.Generic;
using System.Diagnostics;
using Digipost.Signature.Api.Client.Asice;
using Digipost.Signature.Api.Client.Asice.Manifest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.AsiceTests
{
    [TestClass]
    public class SerializeUtilityTests
    {
        [TestClass]
        public class SerializeMethod : SerializeUtilityTests
        {
            [TestInitialize]
            public void TestInitialize()
            {
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            }

            [TestMethod]
            public void SerializeManifestProperly()
            {
                //Arrange
                const string expectedResult = "<?xml version=\"1.0\" encoding=\"utf-8\"?><manifest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://signering.digipost.no/schema/v1/signature-job\"><signers><signer><personal-identification-number>1234567890</personal-identification-number></signer></signers><sender><organization>123456789</organization></sender><primary-document href=\"document.pdf\" mime=\"application/pdf\"><title>Tittel</title><description>Melding til signatar</description></primary-document></manifest>";
                var manifest = new ManifestDataTranferObject()
                {
                    SignersDataTransferObjects = new List<SignerDataTranferObject>()
                    {
                        new SignerDataTranferObject()
                        {
                            PersonalIdentificationNumber = "1234567890"
                        }
                    },
                    SenderDataTransferObject = new SenderDataTransferObject()
                    {
                        Organization = "123456789"
                    },
                    PrimaryDocumentDataTransferObject = new PrimaryDocumentDataTransferObject()
                    {
                        Title = "Tittel",
                        Descritpion = "Melding til signatar",
                        Href = "document.pdf",
                        Mime = "application/pdf"
                    }
                };

                //Act
                var result = SerializeUtility.Serialize(manifest);
                Trace.WriteLine(SerializeUtility.FormatXml(result));
                
                //Assert
                Assert.AreEqual(expectedResult, result);
            }

            [TestMethod]
            public void SerializePrimaryDocumentProperly()
            {
                //Arrange
                const string expectedResult = "<?xml version=\"1.0\" encoding=\"utf-8\"?><primary-document xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" href=\"document.pdf\" mime=\"application/pdf\"><title>Tittel</title><description>Melding til signatar</description></primary-document>";
                var document = new PrimaryDocumentDataTransferObject()
                {
                    Title = "Tittel",
                    Descritpion = "Melding til signatar",
                    Href = "document.pdf",
                    Mime = "application/pdf"
                };

                //Act
                var result = SerializeUtility.Serialize(document);
                Trace.WriteLine(SerializeUtility.FormatXml(result));
                
                //Assert
                Assert.AreEqual(expectedResult, result);
            }
            
            [TestMethod]
            public void SerializeSenderProperly()
            {
                //Arrange
                const string expectedResult = "<?xml version=\"1.0\" encoding=\"utf-8\"?><sender xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><organization>123456789</organization></sender>";
                var sender = new SenderDataTransferObject {Organization = "123456789"};

                //Act
                var result = SerializeUtility.Serialize(sender);
                Trace.WriteLine(SerializeUtility.FormatXml(result));

                //Assert
                Assert.AreEqual(expectedResult, result);
            }

            [TestMethod]
            public void SerializeSignerProperly()
            {
                //Arrange
                const string expectedResult = "<?xml version=\"1.0\" encoding=\"utf-8\"?><signer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><personal-identification-number>1234567890</personal-identification-number></signer>";
                var signer = new SignerDataTranferObject()
                {
                    PersonalIdentificationNumber = "1234567890" 
                };

                //Act
                var result = SerializeUtility.Serialize(signer);
                Trace.WriteLine(SerializeUtility.FormatXml(result));

                //Assert
                Assert.AreEqual(expectedResult, result);
            }
        }
    }
}