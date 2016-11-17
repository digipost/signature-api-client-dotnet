using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class NotificationsUsingLookupTests
    {
        public class SmsIfAvailableProperty : NotificationsUsingLookupTests
        {
            [Fact]
            public void Returns_true()
            {
                //Arrange

                //Act
                var notificationsUsingLookup = new NotificationsUsingLookup();

                //Assert
                Assert.True(notificationsUsingLookup.EmailIfAvailable);
            }
        }
    }
}