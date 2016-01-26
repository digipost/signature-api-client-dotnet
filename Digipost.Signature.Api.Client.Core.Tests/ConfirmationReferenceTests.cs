using System;
using Digipost.Signature.Api.Client.Direct;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass()]
    public class ConfirmationReferenceTests
    {

        [TestClass]
        public class ConstructorMethod : ConfirmationReferenceTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var confirmationUrl = new Uri("http://test.digipost.no/confirmationUrl");

                //Act
                var confirmationReference = new ConfirmationReference(confirmationUrl);

                //Assert
                Assert.AreEqual(confirmationUrl, confirmationReference.ConfirmationUrl);
            }
        }
    }
}