using System;
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
                var cancellationUrl = new Uri("http://cancellationUrl.no");
                var portalJobResponse = new PortalJobResponse(jobId, cancellationUrl);

                //Assert
                Assert.AreEqual(jobId, portalJobResponse.JobId);
                Assert.AreEqual(cancellationUrl, portalJobResponse.CancellationReference.Url);
            }
        }
    }
}