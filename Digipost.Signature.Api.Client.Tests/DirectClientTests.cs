﻿using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Direct;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Tests
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
                var clientConfiguration = new ClientConfiguration();
                var client = new DirectClient(clientConfiguration);

                //Act

                //Assert
                Assert.AreEqual(clientConfiguration, client.ClientConfiguration);
            }
        }
    }
}