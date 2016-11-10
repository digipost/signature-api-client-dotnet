using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Exceptions
{
    [TestClass]
    public class TooEagerPollingExceptionTests
    {
        [TestClass]
        public class ConstructorMethod : TooEagerPollingExceptionTests
        {
            [TestMethod]
            public void InitializeWithDateString()
            {
                //Arrange
                const string source = "2016-11-10T10:39:46.610+01:00";
                var expected = new DateTime(2016, 11, 10, 09, 39, 46, 610, DateTimeKind.Utc);

                //Act
                var tooEagerPollingException = new TooEagerPollingException(source);

                //Assert
                Assert.AreEqual(expected, tooEagerPollingException.NextPermittedPollTime.ToUniversalTime());
            }
        }
    }
}