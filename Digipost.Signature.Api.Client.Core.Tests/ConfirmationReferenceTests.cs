using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class ConfirmationReferenceTests
    {
        public class ConstructorMethod : ConfirmationReferenceTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var confirmationUri = new Uri("http://confirmationUri.no");

                //Act
                var confirmationReference = new ConfirmationReference(confirmationUri);

                //Assert
                Assert.Equal(confirmationUri, confirmationReference.Url);
            }
        }
    }
}
