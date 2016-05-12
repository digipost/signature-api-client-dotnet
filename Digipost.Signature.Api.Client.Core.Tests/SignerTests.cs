using Digipost.Signature.Api.Client.Core.Tests.Stubs;
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
            public void InitializesWithProperties()
            {
                //Arrange
                var personalIdentificationNumber = "01013300001";

                //Act
                var signer = new SignerStub(personalIdentificationNumber);

                //Assert
                Assert.AreEqual(personalIdentificationNumber, signer.PersonalIdentificationNumber);
            }
        }

        [TestClass]
        public class ToStringMethod : SignerTests
        {
            [TestMethod]
            public void ReturnsMaskedPersonalIdentificationNumber()
            {
                //Arrange
                var personalIdentificationNumber = "01013300001";
                var signer = new SignerStub(personalIdentificationNumber);
                var maskedIdentificationNumber = "010133*****";

                //Act
                var toString = signer.ToString();

                //Assert
                Assert.IsTrue(toString.Contains(maskedIdentificationNumber));
                Assert.IsFalse(toString.Contains(personalIdentificationNumber));
            }
        }
    }
}