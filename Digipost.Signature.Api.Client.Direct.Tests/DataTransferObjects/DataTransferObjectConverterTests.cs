using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                var document = Core.Tests.Utilities.CoreDomainUtility.GetDocument();
                var signer = Core.Tests.Utilities.CoreDomainUtility.GetSigner();
                var reference = "reference";
                var exitUrls = DomainUtility.GetExitUrls();

                var source = new DirectJob(
                    document,
                    signer,
                    reference,
                    exitUrls);

                var expected = new directsignaturejobrequest()
                {
                    reference = reference,
                    exiturls = new exiturls()
                    {
                        cancellationurl = exitUrls.CancellationUrl.AbsoluteUri,
                        completionurl = exitUrls.CompletionUrl.AbsoluteUri,
                        errorurl = exitUrls.ErrorUrl.AbsoluteUri
                    }
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

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
                var expected = new exiturls()
                {
                    completionurl = source.CompletionUrl.AbsoluteUri,
                    cancellationurl = source.CancellationUrl.AbsoluteUri,
                    errorurl = source.ErrorUrl.AbsoluteUri
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
                var source = new directsignaturejobresponse
                {
                    signaturejobid = 1444,
                    redirecturl= "https://localhost:9000/redirect/#/c316e5b62df86a5d80e517b3ff4532738a9e7e43d4ae6075d427b1b58355bc63",
                    statusurl = "https://localhost:8443/api/signature-jobs/77/status"
                };

                var expected = new DirectJobResponse(
                    source.signaturejobid, 
                    new ResponseUrls(
                        redirectUrl: new Uri(source.redirecturl), 
                        statusUrl: new Uri(source.statusurl)
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
                var source = new directsignaturejobstatusresponse()
                {
                    signaturejobid = 77,
                    status = directsignaturejobstatus.SIGNED,
                    confirmationurl = "http://signatureRoot.digipost.no/confirmation",
                    xadesurl = "http://signatureRoot.digipost.no/xades",
                    padesurl = "http://signatureRoot.digipost.no/pades"
                };

                var expected = new DirectJobStatusResponse(
                    source.signaturejobid, 
                    JobStatus.Signed,
                    new JobReferences(
                        confirmation: new Uri(source.confirmationurl), 
                        xades: new Uri(source.xadesurl), 
                        pades: new Uri(source.padesurl))
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
                var source = new directsignaturejobstatusresponse()
                {
                    signaturejobid = 77,
                    status = directsignaturejobstatus.CREATED
                };

                var expected = new DirectJobStatusResponse(
                    source.signaturejobid,
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
                byte[] pdfDocumentBytes = Core.Tests.Utilities.CoreDomainUtility.GetPdfDocumentBytes();
                var personalIdentificationNumber = "12345678901";
                var expectedMimeType = "application/pdf";

                var source = new DirectManifest(
                    new Sender(organizationNumberSender),
                    new Document(documentSubject, documentMessage, documentFileName, FileType.Pdf, pdfDocumentBytes),
                    new Signer(personalIdentificationNumber)
                    );

                var expected = new directsignaturejobmanifest()
                {
                    sender = new sender() { organizationnumber = organizationNumberSender },
                    document = new document
                    {
                        title = documentSubject,
                        description = documentMessage,
                        href = documentFileName,
                        mime = expectedMimeType
                    },
                    signer = new signer { personalidentificationnumber = personalIdentificationNumber}
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
                var expected = new sender()
                {
                    organizationnumber = organizationNumber
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

                var expected = new document()
                {
                    title = subject,
                    description = message,
                    href = fileName,
                    mime = "application/pdf"
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
                var expected = new signer()
                {
                    personalidentificationnumber = personalIdentificationNumber
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