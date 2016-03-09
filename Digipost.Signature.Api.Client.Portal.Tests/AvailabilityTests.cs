using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    [TestClass]
    public class AvailabilityTests
    {
        [TestClass]
        public class AvailableForMethod : AvailabilityTests
        {
            [TestMethod]
            public void ReturnsZeroWhenUninitialized()
            {
                //Arrange
                var expectedAvailableSeconds = 0;

                //Act
                var availability = new Availability();

                //Assert
                Assert.AreEqual(expectedAvailableSeconds, availability.AvailableSeconds);
            }

            [TestMethod]
            public void ReturnsSecondsWhenInitialized()
            {
                //Arrange
                var expectedAvailableSeconds = 86400 + 3600 + 60 + 1;

                //Act
                var availability = new Availability {AvailableFor = new TimeSpan(1, 1, 1, 1)};

                //Assert
                Assert.AreEqual(expectedAvailableSeconds, availability.AvailableSeconds);
            }
        }
    }
}