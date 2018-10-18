using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class SenderTests
    {
        public class ConstructorMethod : SenderTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                const string organizationNumber = "123456789";

                //Act
                var sender = new Sender(organizationNumber);

                //Assert
                Assert.Equal(organizationNumber, sender.OrganizationNumber);
            }
        }
    }
}
