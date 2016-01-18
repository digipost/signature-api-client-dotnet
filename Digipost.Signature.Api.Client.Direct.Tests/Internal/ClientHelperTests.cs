using System;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Internal
{
    [TestClass]
    public class ClientHelperTests
    {
        [TestClass]
        public class ConstructorMethod : ClientHelperTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var clientConfiguration = DomainUtility.GetClientConfiguration();

                //Act
                var clientHelper = new ClientHelper(clientConfiguration);

                //Assert
                Assert.AreEqual(clientConfiguration, clientHelper.ClientConfiguration);
            }
        }

        [TestClass]
        public class SendDirectJobRequestMethod : ClientHelperTests
        {
            [TestMethod]
            public void SendsProperRequest()
            {
                //Arrange
                
                //Act

                //Assert
                Assert.Fail();
            } 
        }
    }
}