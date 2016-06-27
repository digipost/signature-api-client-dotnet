using System;
using Digipost.Signature.Api.Client.Core.Exceptions;
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
                var source = "2016-02-16T16:22:23.045+01:00";
                var expected = new DateTime(2016, 02, 16, 16, 22, 23, 045).ToUniversalTime();

                //Act
                var tooEagerPollingException = new TooEagerPollingException(source);

                //Assert
                Assert.AreEqual(expected, tooEagerPollingException.NextPermittedPollTime.ToUniversalTime());
            }
        }
    }
}