using System;
using Digipost.Signature.Api.Client.Core;
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
                var signer = new PersonalIdentificationNumber("12345678910");

                //Act
                var reference = new RedirectReference(url, signer);

                //Assert
                Assert.AreEqual(url, reference.Url);
                Assert.AreEqual(signer, reference.Signer);
            }
        }
    }
}