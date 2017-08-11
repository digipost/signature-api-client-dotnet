using System;
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
                var notifications = new Notifications(new Email("email@example.com"));

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

            [Fact]
            public void InitializesWithSignerIdentifier()
            {
                //Arrange
                var identifier = new Email("email@example.com");

                //Act
                var portalSigner = new Signer(new SignerIdentifier(identifier));

                //Assert
                Assert.Equal(identifier.Address, portalSigner.Identifier);
            }

            [Fact]
            public void Issue99()
            {
                Job job = new Job(
                  null, null, null
                  );
                //Bare for test-formål
                TimeSpan ts = new TimeSpan(14, 0, 0, 0);

                Availability jobAvailable = new Availability();
                jobAvailable.AvailableFor = ts;

                job.Availability = jobAvailable;

                //Har ikke mulighet til å kjøre awaitable task
                //JobResponse x = portalClient.Create(job).Result;
            }

        }
    }
}