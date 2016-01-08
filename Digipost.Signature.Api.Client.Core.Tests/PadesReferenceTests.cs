using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass()]
    public class PadesReferenceTests
    {
        [TestClass]
        public class ConstructorMethod : PadesReferenceTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var uri = new Uri("http://localhost/test");
                var padesReference = new PadesReference(uri);

                //Act

                //Assert
                Assert.AreEqual(uri,padesReference.PadesUri);
            }
        }

    }
}