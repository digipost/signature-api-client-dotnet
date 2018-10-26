using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice
{
    public class DocumentBundleTests
    {
        public class ConstructorMethod : DocumentBundleTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var bundleBytes = new byte[] {0x21, 0x22};

                //Act
                var documentBundle = new DocumentBundle(bundleBytes);

                //Assert
                Assert.Equal(bundleBytes, documentBundle.BundleBytes);
            }
        }
    }
}
