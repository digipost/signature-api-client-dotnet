using System;
using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Internal;
using Digipost.Signature.Api.Client.Direct.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Internal
{
    [TestClass]
    public class CreateActionTests
    {
        [TestClass]
        public class ConstructorMethod : CreateActionTests
        {
            [TestMethod]
            public void InitializesClassAndParentProperties()
            {
                //Arrange
                var sender = DomainUtility.GetSender();
                var document = DomainUtility.GetDocument();
                var enumerable = DomainUtility.GetSigners(1);
                var businessCertificate = DomainUtility.GetTestCertificate();
                var directJob = DomainUtility.GetDirectJob();
                var serializedDirectJob = SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject(directJob));

                var asiceBundle = AsiceGenerator.CreateAsice(sender, document, enumerable, businessCertificate);

                //Act
                var action = new DirectCreateAction(sender,
                        directJob, asiceBundle);
                
                //Assert
                Assert.AreEqual(directJob, action.RequestContent);
                Assert.AreEqual(serializedDirectJob, action.RequestContentXml.InnerXml);

                Assert.AreEqual(null, action.MultipartFormDataContent);
            }
        }

        [TestClass]
        public class SerializeFunc : CreateActionTests
        {
            [TestMethod]
            public void SerializesDirectJob()
            {
                //Arrange
                var directJob = DomainUtility.GetDirectJob();
                var expected = SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject(directJob));

                //Act
                var result = DirectCreateAction.SerializeFunc(directJob);

                //Assert
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void DeserializesDirectJob()
            {
                //Arrange
                var expectedJobId = 77;
                var expectedRedirectUrl = "https://localhost:9000/redirect/#/c316e5b62df86a5d80e517b3ff4532738a9e7e43d4ae6075d427b1b58355bc63";
                var expectedStatusUrl = "https://localhost:8443/api/signature-jobs/77/status";

                var serialized =
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                        "<direct-signature-job-response xmlns=\"http://signering.posten.no/schema/v1\" xmlns:ns2=\"http://uri.etsi.org/01903/v1.3.2#\" xmlns:ns3=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ns4=\"http://uri.etsi.org/2918/v1.2.1#\">" +
                            "<signature-job-id>77</signature-job-id>" +
                            "<redirect-url>https://localhost:9000/redirect/#/c316e5b62df86a5d80e517b3ff4532738a9e7e43d4ae6075d427b1b58355bc63</redirect-url>" +
                            "<status-url>https://localhost:8443/api/signature-jobs/77/status</status-url>" +
                        "</direct-signature-job-response>";

                //Act
                var result = DirectCreateAction.DeserializeFunc(serialized);


                //Assert
                Assert.AreEqual(expectedJobId, result.JobId);
                Assert.AreEqual(new Uri(expectedRedirectUrl), result.ResponseUrls.Redirect);
                Assert.AreEqual(new Uri(expectedStatusUrl), result.ResponseUrls.Status);
            }
        }

        internal DirectCreateAction GetCreateAction()
        {
            var document = DomainUtility.GetDocument();
            var enumerable = DomainUtility.GetSigners(1);
            var businessCertificate = DomainUtility.GetTestCertificate();
            var directJob = DomainUtility.GetDirectJob();

            var asiceBundle = AsiceGenerator.CreateAsice(sender, document, enumerable, businessCertificate);

            return new DirectCreateAction(directJob, asiceBundle);
        }

    }
}