using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class JobReferencesTests
    {
        public class ConstructorMethod : JobReferencesTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var confirmation = new Uri("http://signatureRoot.digipost.no/confirmation");
                var pades = new Uri("http://signatureRoot.digipost.no/pades");

                //Act
                var jobReferences = new JobReferences(confirmation, pades);

                //Assert
                Assert.Equal(confirmation, jobReferences.Confirmation.Url);
                Assert.Equal(pades, jobReferences.Pades.Url);
            }
        }
    }
}