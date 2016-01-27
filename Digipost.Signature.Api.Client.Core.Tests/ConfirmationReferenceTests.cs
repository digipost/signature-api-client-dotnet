using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class ConfirmationReferenceTests
    {
        [TestClass]
        public class ConstructorMethod : ConfirmationReferenceTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var confirmationUri = new Uri("http://confirmationUri.no");

                //Act
                var confirmationReference = new ConfirmationReference(confirmationUri);

                //Assert
                Assert.AreEqual(confirmationUri, confirmationReference.ConfirmationUri);
            }
        }
    }
}