using System;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Exceptions
{
    public class TooEagerPollingExceptionTests
    {
        public class ConstructorMethod : TooEagerPollingExceptionTests
        {
            private const string ExpectedInToString = "2019-04-22T09:39:46.6100000Z";
            private static readonly DateTime ExpectedDateTime = new DateTime(2019, 04, 22, 09, 39, 46, 610, DateTimeKind.Utc);

            [Fact]
            public void Initialize_with_date_string()
            {
                //Arrange
                const string source = "2019-04-22T10:39:46.610+01:00";
                
                //Act
                var tooEagerPollingException = new TooEagerPollingException(source);

                //Assert
                Assert.Equal(ExpectedDateTime, tooEagerPollingException.NextPermittedPollTime.ToUniversalTime());
                Assert.Contains(ExpectedInToString, tooEagerPollingException.Message);
            }
            
            [Fact]
            public void Initialize_with_date_time()
            {
                //Arrange
                var source = new DateTime(2019, 04, 22, 09, 39, 46, 610, DateTimeKind.Utc);
                

                //Act
                var tooEagerPollingException = new TooEagerPollingException(source);

                //Assert
                Assert.Equal(ExpectedDateTime, tooEagerPollingException.NextPermittedPollTime.ToUniversalTime());
                Assert.Contains(ExpectedInToString, tooEagerPollingException.Message);
            }

        }
    }
}
