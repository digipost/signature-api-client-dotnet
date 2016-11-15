using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class JobResponseTests
    {
        public class ConstructorMethod : JobResponseTests
        {
            [Fact]
            public void SSimple_constructor()
            {
                //Arrange
                var jobId = 123456789;
                var responseUrls = DomainUtility.GetResponseUrls();

                //Act
                var directJobResponse = new JobResponse(
                    jobId,
                    responseUrls
                );

                //Assert
                Assert.Equal(jobId, directJobResponse.JobId);
                Assert.Equal(responseUrls, directJobResponse.ResponseUrls);
            }
        }
    }
}