using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{

    [TestClass]
    public class DirectJobStatusResponseTests
    {
        [TestClass]
        public class ConstructorMethod : DirectJobStatusResponseTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var jobId = 22;
                var jobStatus = JobStatus.Cancelled;
                var statusResponseUrls = DomainUtility.GetStatusResponseUrls();

                //Act
                var directJobStatusResponse = new DirectJobStatusResponse(
                    jobId, 
                    jobStatus, 
                    statusResponseUrls
                );

                //Assert
                Assert.AreEqual(jobId, directJobStatusResponse.JobId);
                Assert.AreEqual(jobStatus, directJobStatusResponse.JobStatus);
                Assert.AreEqual(statusResponseUrls, directJobStatusResponse.StatusResponseUrls);
            }
        }

        [TestClass]
        public class ConfirmationReferenceMethod : DirectJobStatusResponseTests
        {
            [TestMethod]
            public void ReturnsSameUrlAsConfirmationUrlInStatusResponse()
            {
                //Arrange
                var directJobStatusResponse = DomainUtility.GetDirectJobStatusResponse();

                //Act

                //Assert
                Assert.AreEqual(directJobStatusResponse.StatusResponseUrls.Confirmation, directJobStatusResponse.ConfirmationReference.ConfirmationUrl);
            } 
        }
    }
}