using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    [TestClass]
    public class JobTests
    {
        [TestClass]
        public class ConstructorMethod : JobTests
        {
            [TestMethod]
            public void ConstructorWithoutSenderExists()
            {
                //Arrange

                //Act
                new Job(null, null, null);

                //Assert
            }

            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var document = DomainUtility.GetPortalDocument();
                var signers = DomainUtility.GetSigners(3);
                var reference = "PortalJobReference";
                var portalJob = new Job(document, signers, reference);

                //Act

                //Assert
                Assert.AreEqual(document, portalJob.Document);
                Assert.AreEqual(signers, portalJob.Signers);
                Assert.AreEqual(reference, portalJob.Reference);
            }
        }
    }
}