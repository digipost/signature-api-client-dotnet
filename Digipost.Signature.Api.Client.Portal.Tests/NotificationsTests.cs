using Digipost.Signature.Api.Client.Core.Identifier;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class NotificationsTests
    {
        public class ConstructorMethod : NotificationsTests
        {
            [Fact]
            public void Initializes_with_enforced_email()
            {
                //Arrange
                var email = new Email("tull@ball.no");

                //Act
                var notifications = new Notifications(email);

                //Assert
                Assert.Equal(email, notifications.Email);
            }

            [Fact]
            public void Initializes_with_enforced_sms()
            {
                //Arrange
                var sms = new Sms("1233456789");

                //Act
                var notifications = new Notifications(sms);

                //Assert
                Assert.Equal(sms, notifications.Sms);
            }

            [Fact]
            public void Initializes_with_sms_and_email_both_orderings()
            {
                //Arrange
                var email = new Email("tull@ball.no");
                var sms = new Sms("1233456789");

                //Act
                var notificationsSmsFirst = new Notifications(sms, email);
                var notificationsEmailFirst = new Notifications(email, sms);

                //Assert
                Assert.Equal(email, notificationsEmailFirst.Email);
                Assert.Equal(email, notificationsSmsFirst.Email);
                Assert.Equal(sms, notificationsSmsFirst.Sms);
                Assert.Equal(sms, notificationsEmailFirst.Sms);
            }
        }
    }
}
