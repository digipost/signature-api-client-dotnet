using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class StatusResponseUrlsTests
    {
        [TestClass]
        public class ConstructorMethod : StatusResponseUrlsTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var confirmation = new Uri("http://signatureRoot.digipost.no/confirmation");
                var xades = new Uri("http://signatureRoot.digipost.no/xades");
                var pades = new Uri("http://signatureRoot.digipost.no/pades");

                //Act
                var statusResponseUrls = new StatusResponseUrls( confirmation, xades, pades);
                
                //Assert
                Assert.AreEqual(confirmation,statusResponseUrls.Confirmation);
                Assert.AreEqual(xades,statusResponseUrls.Xades);
                Assert.AreEqual(pades,statusResponseUrls.Pades);
            }
        }

    }
}
