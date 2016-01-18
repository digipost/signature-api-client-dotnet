using System;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class DirectClientTests
    {
        [TestClass]
        public class ConstructorMethod : DirectClientTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var signatureServiceRoot = new Uri("http://SignatureServiceRoot.no");
                var sender = DomainUtility.GetSender();
                var x509Certificate2 = DomainUtility.GetTestCertificate();
                var clientConfiguration = new ClientConfiguration(
                    signatureServiceRoot, sender, x509Certificate2); 
                
                //Act
                var client = new DirectClient(clientConfiguration);


                //Assert
                Assert.AreEqual(clientConfiguration, client.ClientConfiguration);
            }
        }
    }
}