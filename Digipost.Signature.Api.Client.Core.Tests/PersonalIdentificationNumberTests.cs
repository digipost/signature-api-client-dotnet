using Digipost.Signature.Api.Client.Core.Identifier;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class PersonalIdentificationNumberTests
    {
        public class ToStringMethod : AbstractSignerTests
        {
            [Fact]
            public void Returns_masked_personal_identification_number()
            {
                //Arrange
                var pin = "01013300001";
                var personalIdentificationNumber = new PersonalIdentificationNumber(pin);
                var maskedPersonalIdentificationNumber = "010133*****";

                //Act
                var toString = personalIdentificationNumber.ToString();

                //Assert
                Assert.Contains(maskedPersonalIdentificationNumber, toString);
                Assert.DoesNotContain(pin, toString);
            }
        }
    }
}
