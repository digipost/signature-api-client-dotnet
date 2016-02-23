using Digipost.Signature.Api.Client.Core.Asice;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice
{
    [TestClass]
    public class DocumentBundleTests
    {
        [TestClass]
        public class ConstructorMethod : DocumentBundleTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var bundleBytes = new byte[] {0x21, 0x22};

                //Act
                var documentBundle = new DocumentBundle(bundleBytes);

                //Assert
                Assert.AreEqual(bundleBytes, documentBundle.BundleBytes);
            }
        }
    }
}