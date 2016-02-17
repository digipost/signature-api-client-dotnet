using Digipost.Signature.Api.Client.Direct.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainUtility = Digipost.Signature.Api.Client.Direct.Tests.Utilities.DomainUtility;

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
                var jobReferences = DomainUtility.GetJobReferences();

                //Act
                var directJobStatusResponse = new DirectJobStatusResponse(
                    jobId, 
                    jobStatus, 
                    jobReferences
                );

                //Assert
                Assert.AreEqual(jobId, directJobStatusResponse.JobId);
                Assert.AreEqual(jobStatus, directJobStatusResponse.Status);
                Assert.AreEqual(jobReferences, directJobStatusResponse.References);
            }
        }
    }
}