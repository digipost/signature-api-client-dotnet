using System;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Exceptions
{
    public class TooEagerPollingExceptionTests
    {
        public class ConstructorMethod : TooEagerPollingExceptionTests
        {
            [Fact]
            public void Initialize_with_date_string()
            {
                //Arrange
                const string source = "2016-11-10T10:39:46.610+01:00";
                var expected = new DateTime(2016, 11, 10, 09, 39, 46, 610, DateTimeKind.Utc);

                //Act
                var tooEagerPollingException = new TooEagerPollingException(source);

                //Assert
                Assert.Equal(expected, tooEagerPollingException.NextPermittedPollTime.ToUniversalTime());
            }
        }
    }
}
