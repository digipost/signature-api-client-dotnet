using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class PersonalIdentificationNumberTests
    {
        [TestClass]
        public class ToStringMethod : AbstractSignerTests
        {
            [TestMethod]
            public void ReturnsMaskedPersonalIdentificationNumber()
            {
                //Arrange
                var pin = "01013300001";
                var personalIdentificationNumber = new PersonalIdentificationNumber(pin);
                var maskedPersonalIdentificationNumber = "010133*****";

                //Act
                var toString = personalIdentificationNumber.ToString();

                //Assert
                Assert.IsTrue(toString.Contains(maskedPersonalIdentificationNumber));
                Assert.IsFalse(toString.Contains(pin));
            }
        }
    }
}