using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class SignerTests
    {
        [TestClass]
        public class ConstructorMethod : SignerTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var personalIdentificationNumber = "01013300001";
                var signer = new Signer(personalIdentificationNumber);

                //Act

                //Assert
                Assert.AreEqual(personalIdentificationNumber, signer.PersonalIdentificationNumber);
            }
        }
    }
}