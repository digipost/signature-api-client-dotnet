using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class CancellationReferenceTests
    {
        public class ConstructorMethod : CancellationReferenceTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var url = new Uri("http://testuri");

                //Act
                var cancellationReference = new CancellationReference(url);

                //Assert
                Assert.Equal(url, cancellationReference.Url);
            }
        }
    }
}
