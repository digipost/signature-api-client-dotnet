using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class JobStatusResponseTests
    {
        public class ConstructorMethod : JobStatusResponseTests
        {
            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                var jobId = 22;
                var jobReference = "senders-reference";
                var jobStatus = JobStatus.Failed;
                var jobReferences = DomainUtility.GetJobReferences();
                var signatures = DomainUtility.GetSignatures(1);

                //Act
                var jobStatusResponse = new JobStatusResponse(
                    jobId,
                    jobReference,
                    jobStatus,
                    jobReferences,
                    signatures
                );

                //Assert
                Assert.Equal(jobId, jobStatusResponse.JobId);
                Assert.Equal(jobReference, jobStatusResponse.JobReference);
                Assert.Equal(jobStatus, jobStatusResponse.Status);
                Assert.Equal(jobReferences, jobStatusResponse.References);
                Assert.Equal(signatures, jobStatusResponse.Signatures);
            }
        }
    }
}