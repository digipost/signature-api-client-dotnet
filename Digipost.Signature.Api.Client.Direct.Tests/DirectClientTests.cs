using System;
using System.Net.Http;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Tests.Fakes;
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
                var clientConfiguration = CoreDomainUtility.GetClientConfiguration();
         
                //Act
                var client = new DirectClient(clientConfiguration);

                //Assert
                Assert.AreEqual(clientConfiguration, client.ClientConfiguration);
                Assert.IsNotNull(client.HttpClient);
                Assert.IsNotNull(client.HttpClient);
            }
        }
    }
}