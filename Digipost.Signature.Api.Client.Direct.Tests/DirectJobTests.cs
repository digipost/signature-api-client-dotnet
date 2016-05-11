using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class DirectJobTests
    {
        [TestClass]
        public class ConstructorMethod : DirectJobTests
        {
            [TestMethod]
            public void ConstructorWithoutSenderExists()
            {
                //Act
                new DirectJob(null, null, null, null);

                //Assert
            }

            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var id = "IdDirectJob";
                var signer = new Signer("01013300001");
                var document = DomainUtility.GetDirectDocument();
                var exitUrls = DomainUtility.GetExitUrls();
                var sender = CoreDomainUtility.GetSender();

                //Act
                var directJob = new DirectJob(document, signer, id, exitUrls, sender);

                //Assert
                Assert.AreEqual(id, directJob.Reference);
                Assert.AreEqual(signer, directJob.Signer);
                Assert.AreEqual(document, directJob.Document);
                Assert.AreEqual(exitUrls, directJob.ExitUrls);
                Assert.AreEqual(sender, directJob.Sender);
            }
        }
    }
}