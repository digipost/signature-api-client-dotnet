using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    [TestClass]
    public class PortalJobTests
    {
        [TestClass]
        public class ConstructorMethod : PortalJobTests
        {
            [TestMethod]
            public void ConstructorWithoutSenderExists()
            {
                //Arrange

                //Act
                new PortalJob(null, null, null);

                //Assert
            }

            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var document = CoreDomainUtility.GetDocument();
                var signers = CoreDomainUtility.GetSigners(3);
                var reference = "PortalJobReference";
                var portalJob = new PortalJob(document, signers, reference);

                //Act

                //Assert
                Assert.AreEqual(document, portalJob.Document);
                Assert.AreEqual(signers, portalJob.Signers);
                Assert.AreEqual(reference, portalJob.Reference);
            }
        }
    }
}