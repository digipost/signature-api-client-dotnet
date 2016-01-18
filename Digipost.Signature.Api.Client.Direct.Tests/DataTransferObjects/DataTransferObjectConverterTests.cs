using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataTransferObjectConverter = Digipost.Signature.Api.Client.Direct.DataTransferObjects.DataTransferObjectConverter;

namespace Digipost.Signature.Api.Client.Direct.Tests.DataTransferObjects
{
    [TestClass()]
    public class DataTransferObjectConverterTests
    {
        [TestClass]
        public class ToDataTranferObjectMethod : DataTransferObjectConverterTests
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

                var expected = new DirectJobDataTranferObject()
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
                    DocumentDataTransferObject = new DocumentDataTransferObject()
                    {
                        Title = document.Subject,
                        Description = document.Message,
                        Mime = document.MimeType,
                        Href = document.FileName
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
    }
}