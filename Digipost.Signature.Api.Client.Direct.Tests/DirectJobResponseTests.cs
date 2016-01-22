
using System;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class DirectJobResponseTests
    {
        [TestClass]
        public class ConstructorMethod : DirectJobResponseTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var jobId = 123456789;
                var responseUrls = DomainUtility.GetResponseUrls();

                //Act
                var directJobResponse = new DirectJobResponse(
                    jobId, 
                    responseUrls
                    );

                //Assert
                Assert.AreEqual(jobId,directJobResponse.JobId);
                Assert.AreEqual(responseUrls, directJobResponse.ResponseUrls);
            }
        }

        [TestClass]
        public class DirectJobReferenceMethod : DirectJobResponseTests
        {
            [TestMethod]
            public void ReturnsJobReferenceWithStatusUrl()
            {
                //Arrange
                var directJobResponse = DomainUtility.GetDirectJobResponse();

                //Act
                
                //Assert
                Assert.AreEqual(directJobResponse.ResponseUrls.Status, directJobResponse.DirectJobReference.Reference);
            }
        }
    }

}