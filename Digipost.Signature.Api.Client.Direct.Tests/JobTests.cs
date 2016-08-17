using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
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
                //Act
                new Job(null, null, null, null);

                //Assert
            }

            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var id = "IdDirectJob";
                var signers = DomainUtility.GetSigner();
                var document = DomainUtility.GetDirectDocument();
                var exitUrls = DomainUtility.GetExitUrls();
                var sender = CoreDomainUtility.GetSender();

                //Act
                var directJob = new Job(document, signers, id, exitUrls, sender);

                //Assert
                Assert.AreEqual(id, directJob.Reference);
                Assert.AreEqual(signers, directJob.Signers);
                Assert.AreEqual(document, directJob.Document);
                Assert.AreEqual(exitUrls, directJob.ExitUrls);
                Assert.AreEqual(sender, directJob.Sender);
            }
        }
    }
}