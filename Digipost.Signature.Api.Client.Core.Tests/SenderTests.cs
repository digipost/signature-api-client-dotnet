using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class SenderTests
    {
        [TestClass]
        public class ConstructorMethod : SenderTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                const string organizationNumber = "123456789";

                //Act
                var sender = new Sender(organizationNumber);

                //Assert
                Assert.AreEqual(organizationNumber, sender.OrganizationNumber);
            }
        }
    }

}