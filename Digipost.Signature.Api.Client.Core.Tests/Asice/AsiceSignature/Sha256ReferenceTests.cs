using Digipost.Signature.Api.Client.Core.Internal.Asice.AsiceSignature;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice.AsiceSignature
{
    public class Sha256ReferenceTests
    {
        public class ConstructorMethod : Sha256ReferenceTests
        {
            [Fact]
            public void Bytes_constructor()
            {
                //Arrange
                var reference = new Sha256Reference(new byte[] {0xb, 0xc});
                var digestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";

                //Act

                //Assert
                Assert.Equal(digestMethod, reference.DigestMethod);
            }

            [Fact]
            public void Uri_constructor()
            {
                //Arrange
                var reference = new Sha256Reference("uri");
                var digestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";

                //Act

                //Assert
                Assert.Equal(digestMethod, reference.DigestMethod);
            }
        }
    }
}