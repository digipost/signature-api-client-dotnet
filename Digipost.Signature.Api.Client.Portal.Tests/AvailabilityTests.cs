using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class AvailabilityTests
    {
        public class AvailableForMethod : AvailabilityTests
        {
            [Fact]
            public void Returns_null_when_uninitialized()
            {
                //Arrange
                object expectedAvailableSeconds = null;

                //Act
                var availability = new Availability();

                //Assert
                Assert.Equal(expectedAvailableSeconds, availability.AvailableSeconds);
            }

            [Fact]
            public void Returns_seconds_when_initialized()
            {
                //Arrange
                var expectedAvailableSeconds = 86400 + 3600 + 60 + 1;

                //Act
                var availability = new Availability {AvailableFor = new TimeSpan(1, 1, 1, 1)};

                //Assert
                Assert.Equal(expectedAvailableSeconds, availability.AvailableSeconds);
            }
        }
    }
}
