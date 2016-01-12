using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice.DataTransferObjects
{
    [TestClass]
    public class DataTransferObjectConverterTests
    {
        [TestClass]
        public class ToDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [TestMethod]
            public void ConvertsManifestSuccessfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";
                const string documentSubject = "Subject";
                const string documentMessage = "Message";
                const string documentFileName = "Filename.pdf";
                Func<byte[]> pdfDocumentBytes = DomainUtility.GetPdfDocumentBytes;
                var personalIdentificationNumber = "12345678901";
                var expectedMimeType = "application/pdf";

                var source = new Manifest(
                    new Sender(organizationNumberSender),
                    new Document(documentSubject, documentMessage, documentFileName, FileType.Pdf, pdfDocumentBytes()),
                    new List<Signer>(new List<Signer> { new Signer(personalIdentificationNumber) })
                    );

                var expected = new ManifestDataTranferObject()
                {
                    SenderDataTransferObject = new SenderDataTransferObject() { Organization = organizationNumberSender },
                    DocumentDataTransferObject = new DocumentDataTransferObject()
                    {
                        Title = documentSubject,
                        Descritpion = documentMessage,
                        Href = documentFileName,
                        Mime = expectedMimeType
                    },
                    SignersDataTransferObjects = new List<SignerDataTranferObject>()
                    {
                        new SignerDataTranferObject() {PersonalIdentificationNumber = personalIdentificationNumber}
                    }

                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsSenderSuccessfully()
            {
                //Arrange
                const string organizationNumber = "123456789";

                var source = new Sender(organizationNumber);
                var expected = new SenderDataTransferObject
                {
                    Organization = organizationNumber
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsDocumentSuccessfully()
            {
                //Arrange
                const string subject = "Subject";
                const string message = "Message";
                const string fileName = "FileName";
                const FileType fileType = FileType.Pdf;
                var documentBytes = new byte[] { 0x21, 0x22 };

                var source = new Document(
                    subject,
                    message,
                    fileName,
                    fileType,
                    documentBytes
                    );
                var expected = new DocumentDataTransferObject()
                {
                    Title = subject,
                    Descritpion = message,
                    Href = fileName,
                    Mime = "application/pdf"
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsSignerSuccessfully()
            {
                //Arrange
                const string personalIdentificationNumber = "0123456789";

                var source = new Signer(personalIdentificationNumber);
                var expected = new SignerDataTranferObject()
                {
                    PersonalIdentificationNumber = personalIdentificationNumber
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsSignersSuccessfully()
            {
                //Arrange
                const string personalIdentificationNumber1 = "012345678910";
                const string personalIdentificationNumber2 = "012345678911";
                const string personalIdentificationNumber3 = "012345678912";

                var source = new List<Signer>
                {
                    new Signer(personalIdentificationNumber1),
                    new Signer(personalIdentificationNumber2),
                    new Signer(personalIdentificationNumber3),
                };
                var expected = new List<SignerDataTranferObject>
                {
                    new SignerDataTranferObject() {PersonalIdentificationNumber = personalIdentificationNumber1},
                    new SignerDataTranferObject() {PersonalIdentificationNumber = personalIdentificationNumber2},
                    new SignerDataTranferObject() {PersonalIdentificationNumber = personalIdentificationNumber3},
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }
        }
    }
}