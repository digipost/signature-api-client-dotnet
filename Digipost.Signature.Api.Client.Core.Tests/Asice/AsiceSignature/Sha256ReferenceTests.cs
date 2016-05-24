using Digipost.Signature.Api.Client.Core.Internal.Asice.AsiceSignature;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice.AsiceSignature
{
    [TestClass]
    public class Sha256ReferenceTests
    {
        [TestClass]
        public class ConstructorMethod : Sha256ReferenceTests
        {
            [TestMethod]
            public void UriConstructor()
            {
                //Arrange
                var reference = new Sha256Reference("uri");
                var digestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";

                //Act

                //Assert
                Assert.AreEqual(digestMethod, reference.DigestMethod);
            }

            [TestMethod]
            public void BytesConstructor()
            {
                //Arrange
                var reference = new Sha256Reference(new byte[] {0xb, 0xc});
                var digestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";

                //Act

                //Assert
                Assert.AreEqual(digestMethod, reference.DigestMethod);
            }
        }
    }
}