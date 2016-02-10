using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class JobReferencesTests
    {
        [TestClass]
        public class ConstructorMethod : JobReferencesTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var confirmation = new Uri("http://signatureRoot.digipost.no/confirmation");
                var xades = new Uri("http://signatureRoot.digipost.no/xades");
                var pades = new Uri("http://signatureRoot.digipost.no/pades");

                //Act
                var jobReferences = new JobReferences(confirmation, xades, pades);
                
                //Assert
                Assert.AreEqual(confirmation,jobReferences.Confirmation.ConfirmationUri);
                Assert.AreEqual(xades,jobReferences.Xades.Url);
                Assert.AreEqual(pades,jobReferences.Pades.Url);
            }
        }

    }
}
