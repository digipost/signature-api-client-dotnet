using Digipost.Signature.Api.Client.Core;
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
                var signer = new Signer(new PersonalIdentificationNumber("01013300001"));
                var document = DomainUtility.GetDirectDocument();
                var exitUrls = DomainUtility.GetExitUrls();
                var sender = CoreDomainUtility.GetSender();

                //Act
                var directJob = new Job(document, signer, id, exitUrls, sender);

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