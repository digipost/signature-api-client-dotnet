using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
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
                var jobId = 123456789;
                var responseUrls = DomainUtility.GetResponseUrls();

                //Act
                var directJobResponse = new JobResponse(
                    jobId,
                    responseUrls
                    );

                //Assert
                Assert.AreEqual(jobId, directJobResponse.JobId);
                Assert.AreEqual(responseUrls, directJobResponse.ResponseUrls);
            }
        }
    }
}