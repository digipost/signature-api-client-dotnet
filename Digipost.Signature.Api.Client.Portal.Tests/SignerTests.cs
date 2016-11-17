using Digipost.Signature.Api.Client.Core;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class SignerTests
    {
        public class ConstructorMethod : SignerTests
        {
            [Fact]
            public void InitializesWithNotifications()
            {
                //Arrange
                var notifications = new Notifications(new Email("email@address.no"));

                //Act
                var portalSigner = new Signer(new PersonalIdentificationNumber("99999999999"), notifications);

                //Assert
                Assert.Equal(notifications, portalSigner.Notifications);
            }

            [Fact]
            public void InitializesWithNotificationsUsingLookup()
            {
                //Arrange
                var notificationsUsingLookup = new NotificationsUsingLookup();

                //Act
                var portalSigner = new Signer(new PersonalIdentificationNumber("999999999"), notificationsUsingLookup);

                //Assert
                Assert.Equal(notificationsUsingLookup, portalSigner.NotificationsUsingLookup);
            }
        }
    }
}