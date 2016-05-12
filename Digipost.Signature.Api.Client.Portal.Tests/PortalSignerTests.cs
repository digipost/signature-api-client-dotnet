using Digipost.Signature.Api.Client.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    [TestClass]
    public class PortalSignerTests
    {
        [TestClass]
        public class ConstructorMethod : PortalSignerTests
        {
            [TestMethod]
            public void InitializesWithNotifications()
            {
                //Arrange
                var notifications = new Notifications(new Email("email@address.no"));

                //Act
                var portalSigner = new PortalSigner(new PersonalIdentificationNumber("99999999999"), notifications);

                //Assert
                Assert.AreEqual(notifications, portalSigner.Notifications);
            }

            [TestMethod]
            public void InitializesWithNotificationsUsingLookup()
            {
                //Arrange
                var notificationsUsingLookup = new NotificationsUsingLookup();
                
                //Act
                var portalSigner = new PortalSigner(new PersonalIdentificationNumber("999999999"), notificationsUsingLookup);

                //Assert
                Assert.AreEqual(notificationsUsingLookup, portalSigner.NotificationsUsingLookup);
            }
        }
    }
}
