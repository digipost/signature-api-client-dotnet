using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class CancellationReferenceTests
    {
        [TestClass]
        public class ConstructorMethod : CancellationReferenceTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var url = new Uri("http://testuri");

                //Act
                var cancellationReference = new CancellationReference(url);

                //Assert
                Assert.AreEqual(url, cancellationReference.Url);
            }
        }
    }
}