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

                var result = new DirectJobDataTranferObject()
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
                        CancellationUrl = exitUrls.CancellationUrl,
                        CompletionUrl = exitUrls.CompletionUrl,
                        ErrorUrl = exitUrls.ErrorUrl
                    }
                };

                //Act
                var expected = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }
        }
    }
}