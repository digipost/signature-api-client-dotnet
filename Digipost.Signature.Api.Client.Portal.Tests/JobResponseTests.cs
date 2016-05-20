using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    [TestClass]
    public class JobResponseTests
    {
        [TestClass]
        public class ConstructorMethod : JobResponseTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var jobId = 123456789101112;

                //Act
                var cancellationUrl = new Uri("http://cancellationUrl.no");
                var jobResponse = new JobResponse(jobId, cancellationUrl);

                //Assert
                Assert.AreEqual(jobId, jobResponse.JobId);
                Assert.AreEqual(cancellationUrl, jobResponse.CancellationReference.Url);
            }
        }
    }
}