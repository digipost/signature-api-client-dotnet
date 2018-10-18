using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class JobResponseTests
    {
        public class ConstructorMethod : JobResponseTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var jobId = 123456789;
                var jobReference = "senders-reference";
                var responseUrls = DomainUtility.GetResponseUrls();

                //Act
                var directJobResponse = new JobResponse(
                    jobId,
                    jobReference,
                    responseUrls
                );

                //Assert
                Assert.Equal(jobId, directJobResponse.JobId);
                Assert.Equal(jobReference, directJobResponse.JobReference);
                Assert.Equal(responseUrls, directJobResponse.ResponseUrls);
            }
        }
    }
}
