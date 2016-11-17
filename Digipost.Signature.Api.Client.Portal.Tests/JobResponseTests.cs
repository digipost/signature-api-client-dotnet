using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class JobResponseTests
    {
        public class ConstructorMethod : JobResponseTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var jobId = 123456789101112;

                //Act
                var cancellationUrl = new Uri("http://cancellationUrl.no");
                var jobResponse = new JobResponse(jobId, cancellationUrl);

                //Assert
                Assert.Equal(jobId, jobResponse.JobId);
                Assert.Equal(cancellationUrl, jobResponse.CancellationReference.Url);
            }
        }
    }
}