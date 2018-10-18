using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class ExitUrlsTests
    {
        public class ConstructorMethod : ExitUrlsTests
        {
            [Fact]
            public void Simple_contructor()
            {
                //Arrange
                var completionUrl = new Uri("http://localhost/completion");
                var cancellationUrl = new Uri("http://localhost/cancellation");
                var errorUrl = new Uri("http://localhost/error");

                //Act
                var exitUrls = new ExitUrls(completionUrl, cancellationUrl, errorUrl);

                //Assert
                Assert.Equal(completionUrl, exitUrls.CompletionUrl);
                Assert.Equal(cancellationUrl, exitUrls.RejectionUrl);
                Assert.Equal(errorUrl, exitUrls.ErrorUrl);
            }
        }
    }
}
