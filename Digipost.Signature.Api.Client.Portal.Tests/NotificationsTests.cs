using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    [TestClass]
    public class NotificationsTests
    {
        [TestClass]
        public class ConstructorMethod : NotificationsTests
        {
            [TestMethod]
            public void InitializesWithEnforcedEmail()
            {
                //Arrange
                var email = new Email("tull@ball.no");

                //Act
                var notifications = new Notifications(email);

                //Assert
                Assert.AreEqual(email, notifications.Email);
            }

            [TestMethod]
            public void InitializesWithEnforcedSms()
            {
                //Arrange
                var sms = new Sms("1233456789");

                //Act
                var notifications = new Notifications(sms);

                //Assert
                Assert.AreEqual(sms, notifications.Sms);
            }

            [TestMethod]
            public void InitializesWithSmsAndEmailBothOrderings()
            {
                //Arrange
                var email = new Email("tull@ball.no");
                var sms = new Sms("1233456789");

                //Act
                var notificationsSmsFirst = new Notifications(sms, email);
                var notificationsEmailFirst = new Notifications(email, sms);
                
                //Assert
                Assert.AreEqual(email, notificationsEmailFirst.Email);
                Assert.AreEqual(email, notificationsSmsFirst.Email);
                Assert.AreEqual(sms, notificationsSmsFirst.Sms);
                Assert.AreEqual(sms, notificationsEmailFirst.Sms);
            }
        }
    }

}