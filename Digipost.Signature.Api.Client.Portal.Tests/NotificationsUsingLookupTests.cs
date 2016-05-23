using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    [TestClass]
    public class NotificationsUsingLookupTests
    {
        [TestClass]
        public class SmsIfAvailableProperty : NotificationsUsingLookupTests
        {
            [TestMethod]
            public void ReturnsTrue()
            {
                //Arrange

                //Act
                var notificationsUsingLookup = new NotificationsUsingLookup();

                //Assert
                Assert.IsTrue(notificationsUsingLookup.EmailIfAvailable);
            }
        }
    }
}