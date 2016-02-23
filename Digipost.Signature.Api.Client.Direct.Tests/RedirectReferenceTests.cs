using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class RedirectReferenceTests
    {
        [TestClass]
        public class ConstructorMethod : RedirectReferenceTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var url = new Uri("http://redirect.no");

                //Act
                var reference = new RedirectReference(url);

                //Assert
                Assert.AreEqual(url, reference.Url);
            }
        }
    }
}