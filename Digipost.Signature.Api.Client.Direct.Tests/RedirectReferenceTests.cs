using System;
using Digipost.Signature.Api.Client.Core.Identifier;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class RedirectReferenceTests
    {
        public class ConstructorMethod : RedirectReferenceTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var url = new Uri("http://redirect.no");
                var signer = new PersonalIdentificationNumber("12345678910");

                //Act
                var reference = new RedirectReference(url, signer);

                //Assert
                Assert.Equal(url, reference.Url);
                Assert.Equal(signer, reference.Signer);
            }
        }
    }
}
