using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class StatusReferenceTests
    {
        [TestClass]
        public class ConstructorMethod : StatusReferenceTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var urlWithoutToken = new Uri("http://organizationdomain.com/completionUrl/");
                var statusQueryToken = "ALongToken";

                //Act
                var statusReference = new StatusReference(urlWithoutToken, statusQueryToken);

                //Assert
                //Assert.AreEqual();
            }
        }
    }
}