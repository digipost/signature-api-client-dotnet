using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataTransferObjectConverter = Digipost.Signature.Api.Client.Direct.DataTransferObjects.DataTransferObjectConverter;

namespace Digipost.Signature.Api.Client.Direct.Tests.DataTransferObjects
{
    [TestClass]
    public class DataTransferObjectConverterTests
    {
        [TestClass]
        public class ToDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [TestMethod]
            public void ConvertsDirectJobSuccessfully()
            {
                var sender = DomainUtility.GetSender();
                var document = DomainUtility.GetDocument();
                var signer = DomainUtility.GetSigner();
                var reference = "reference";
                var exitUrls = DomainUtility.GetExitUrls();

                var source = new DirectJob(
                    document,
                    signer,
                    reference,
                    exitUrls);

                var expected = new DirectJobDataTransferObject()
                {
                    Reference = reference,
                    SenderDataTransferObject = new SenderDataTransferObject()
                    {
                        Organization = sender.OrganizationNumber
                    },
                    SignerDataTranferObject = new SignerDataTranferObject()
                    {
                        PersonalIdentificationNumber = signer.PersonalIdentificationNumber
                    },
                    ExitUrlsDataTranferObject = new ExitUrlsDataTranferObject()
                    {
                        CancellationUrl = exitUrls.CancellationUrl.AbsoluteUri,
                        CompletionUrl = exitUrls.CompletionUrl.AbsoluteUri,
                        ErrorUrl = exitUrls.ErrorUrl.AbsoluteUri
                    }
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source, sender);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result , out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsExitUrlsSuccessfully()
            {
                //Arrange
                var source = DomainUtility.GetExitUrls();
                var expected = new ExitUrlsDataTranferObject()
                {
                    CompletionUrl = source.CompletionUrl.AbsoluteUri,
                    CancellationUrl = source.CancellationUrl.AbsoluteUri,
                    ErrorUrl = source.ErrorUrl.AbsoluteUri
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

        [TestClass]
        public class FromDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [TestMethod]
            public void ConvertsDirectJobSuccessfully()
            {
                //Arrange
                var source = new DirectJobResponseDataTransferObject()
                {
                    SignatureJobId = "77",
                    RedirectUrl = "https://localhost:9000/redirect/#/c316e5b62df86a5d80e517b3ff4532738a9e7e43d4ae6075d427b1b58355bc63",
                    StatusUrl = "https://localhost:8443/api/signature-jobs/77/status"
                };

                var jobId = Int64.Parse(source.SignatureJobId);

                var expected = new DirectJobResponse(
                    jobId, 
                    new ResponseUrls(
                        redirectUrl: new Uri(source.RedirectUrl), 
                        statusUrl: new Uri(source.StatusUrl)
                        )
                    );

                //Act
                var result = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsSignedDirectJobStatusSuccessfully()
            {
                //Arrange
                var source = new DirectJobStatusResponseDataTransferObject()
                {
                    JobId = "77",
                    Status = "SIGNED",
                    ComfirmationUrl = "http://signatureRoot.digipost.no/confirmation",
                    XadesUrl = "http://signatureRoot.digipost.no/xades",
                    PadesUrl = "http://signatureRoot.digipost.no/pades"
                };

                var jobId = Int64.Parse(source.JobId);

                var expected = new DirectJobStatusResponse(
                    jobId, 
                    JobStatus.Signed,
                    new JobReferences(
                        confirmation: new Uri(source.ComfirmationUrl), 
                        xades: new Uri(source.XadesUrl), 
                        pades: new Uri(source.PadesUrl))
                      );
                        
                //Act
                var result = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsUnsignedDirectJobStatusSuccessfully()
            {
                //Arrange
                var source = new DirectJobStatusResponseDataTransferObject()
                {
                    JobId = "77",
                    Status = "CREATED"
                };

                var jobId = Int64.Parse(source.JobId);

                var expected = new DirectJobStatusResponse(
                    jobId,
                    JobStatus.Created,
                    new JobReferences(
                        confirmation: null,
                        xades: null,
                        pades: null)
                      );

                //Act
                var result = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }
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
                        Description = documentMessage,
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
                    Description = message,
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