using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class StatusReference
    {
        [TestClass]
        public class ConstructorMethod : StatusReference
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var jobReference = new Uri("http://signatureserviceroot.digipost.no/urlidurl/id030302");

                //Act
                var directJobReference = new Direct.StatusReference(jobReference);

                //Assert
                Assert.AreEqual(jobReference,directJobReference.Reference);
            }
        }
    }

}