using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
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

        [TestClass]
        public class GetStatusMethod : DirectClientTests
        {
            [TestMethod]
            public async Task ReturnsStatusResponse()
            {
                //Arrange
                var directClient = new DirectClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = new HttpClient(new FakeHttpClientHandlerGetStatusResponse())
                };

                //Act
                var result = await directClient.GetStatus(new StatusReference(new Uri("http://statusReference.no")));

                //Assert
                Assert.IsNotNull(result);
            }

            [TestMethod]
            [ExpectedException(typeof (SignatureException), AllowDerivedTypes = true)]
            public async Task GetStatusReturnsClass()
            {
                //Arrange
                var directClient = new DirectClient(CoreDomainUtility.GetClientConfiguration())
                {
                    HttpClient = new HttpClient(new FakeHttpClientHandlerForInternalServerErrorResponse())
                };

                //Act
                await directClient.GetStatus(new StatusReference(new Uri("http://statusReference.no")));

                //Assert
                Assert.Fail();
            }
        }
    }
}