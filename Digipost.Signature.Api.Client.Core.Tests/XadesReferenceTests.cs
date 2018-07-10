using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class XadesReferenceTests
    {
        public class ConstructorMethod : XadesReferenceTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var url = new Uri("http://localhost/test");
                var reference = new XadesReference(url);

                //Act

                //Assert
                Assert.Equal(url, reference.Url);
            }
        }
    }
}