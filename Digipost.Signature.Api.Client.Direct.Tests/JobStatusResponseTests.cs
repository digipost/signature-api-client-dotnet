using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class JobStatusResponseTests
    {
        [TestClass]
        public class ConstructorMethod : JobStatusResponseTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var jobId = 22;
                var jobStatus = JobStatus.Failed;
                var jobReferences = DomainUtility.GetJobReferences();

                //Act
                var jobStatusResponse = new JobStatusResponse(
                    jobId,
                    jobStatus,
                    jobReferences
                    );

                //Assert
                Assert.AreEqual(jobId, jobStatusResponse.JobId);
                Assert.AreEqual(jobStatus, jobStatusResponse.Status);
                Assert.AreEqual(jobReferences, jobStatusResponse.References);
            }
        }
    }
}