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
                var jobReference = "senders-reference";
                var cancellationUrl = new Uri("http://cancellationUrl.no");

                //Act
                var jobResponse = new JobResponse(jobId, jobReference, cancellationUrl);

                //Assert
                Assert.Equal(jobId, jobResponse.JobId);
                Assert.Equal(jobReference, jobResponse.JobReference);
                Assert.Equal(cancellationUrl, jobResponse.CancellationReference.Url);
            }
        }
    }
}