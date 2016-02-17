using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass()]
    public class XadesReferenceTests
    {
        [TestClass]
        public class ConstructorMethod : XadesReferenceTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var url = new Uri("http://localhost/test");
                var reference = new XadesReference(url);

                //Act

                //Assert
                Assert.AreEqual(url, reference.Url);
            } 
        }
    }
}