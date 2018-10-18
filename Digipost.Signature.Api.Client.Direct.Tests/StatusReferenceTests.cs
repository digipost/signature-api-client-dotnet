using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class StatusReferenceTests
    {
        public class ConstructorMethod : StatusReferenceTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var urlWithoutToken = new Uri("http://organizationdomain.com/completionUrl/");
                var statusQueryToken = "ALongToken";

                //Act
                var statusReference = new StatusReference(urlWithoutToken, statusQueryToken);

                //Assert
                Assert.Equal(urlWithoutToken, statusReference.BaseUrl);
                Assert.Equal(statusQueryToken, statusReference.StatusQueryToken);
            }
        }
    }
}
