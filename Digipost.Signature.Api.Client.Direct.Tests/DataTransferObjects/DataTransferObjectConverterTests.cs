using System;
using System.Collections.Generic;
using System.Linq;
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
                    sender,
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

        }

    }
}