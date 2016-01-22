using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class DirectJobReferenceTests
    {
        [TestClass]
        public class ConstructorMethod : DirectJobReferenceTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var jobReference = new Uri("http://signatureserviceroot.digipost.no/urlidurl/id030302");

                //Act
                var directJobReference = new DirectJobReference(jobReference);

                //Assert
                Assert.AreEqual(jobReference,directJobReference.Reference);
            }
        }
    }

}