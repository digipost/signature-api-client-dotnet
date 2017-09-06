using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class AbstractSignerTests
    {
        public class ConstructorMethod : AbstractSignerTests
        {
            [Fact]
            public void Initializes_with_properties()
            {
                //Arrange
                var personalIdentificationNumber = new PersonalIdentificationNumber("01013300001");

                //Act
                var signer = new SignerStub(personalIdentificationNumber);

                //Assert
                Assert.True(personalIdentificationNumber.IsSameAs(signer.Identifier), $"Actual {nameof(PersonalIdentificationNumber)}  was not same as initialized value.");
            }
        }
    }
}