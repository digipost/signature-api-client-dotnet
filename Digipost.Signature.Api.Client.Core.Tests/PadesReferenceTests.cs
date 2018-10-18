using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class PadesReferenceTests
    {
        public class ConstructorMethod : PadesReferenceTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var uri = new Uri("http://localhost/test");
                var padesReference = new PadesReference(uri);

                //Act

                //Assert
                Assert.Equal(uri, padesReference.Url);
            }
        }
    }
}
