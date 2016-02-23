using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    [TestClass]
    public class PortalJobResponseTests
    {
        [TestClass]
        public class ConstructorMethod : PortalJobResponseTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var jobId = 123456789101112;

                //Act
                var portalJobResponse = new PortalJobResponse(jobId);

                //Assert
                Assert.AreEqual(jobId, portalJobResponse.JobId);
            }
        }
    }
}