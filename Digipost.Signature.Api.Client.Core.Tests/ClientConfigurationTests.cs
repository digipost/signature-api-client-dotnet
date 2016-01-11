using System;
using System.Runtime.CompilerServices;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class ClientConfigurationTests
    {
        [TestClass]
        public class ConstructorMethod : ClientConfigurationTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var signatureServiceRoot = new Uri("http://SignatureServiceRoot.no");
                var sender = DomainUtility.GetSender();
                var x509Certificate = DomainUtility.GetCertificate();

                //Act
                var clientConfiguration = new ClientConfiguration(
                    signatureServiceRoot, 
                    sender, 
                    x509Certificate
                    );
                
                //Assert
                Assert.AreEqual(signatureServiceRoot, clientConfiguration.SignatureServiceRoot);
                Assert.AreEqual(sender, clientConfiguration.Sender);
                Assert.AreEqual(x509Certificate, clientConfiguration.Certificate);
            }
        }


    }
}