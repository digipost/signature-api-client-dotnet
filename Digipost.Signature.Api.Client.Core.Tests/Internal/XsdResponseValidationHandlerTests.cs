using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Internal
{
    [TestClass]
    public class XsdResponseValidationHandlerTests
    {
        [TestClass]
        public class CreateAsync : XsdResponseValidationHandlerTests
        {
          
            private static HttpClient GetClientWithResponseValidator(DelegatingHandler lastHandler)
            {
                var client = HttpClientFactory.Create(
                    new XsdResponseValidationHandler(),
                    lastHandler
                    );
                return client;
            }


            [TestMethod]
            public async Task AcceptsValidXml()
            {
                //Arrange
                var client = GetClientWithResponseValidator(new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetStatusResponse()));
                await client.GetAsync("http://bogusuri.no");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidXmlException))]
            public async Task ThrowsExceptionOnInvalidXml()
            {
                //Arrange
                var client = GetClientWithResponseValidator(new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetStatusResponseInvalid()));

                //Act
                await client.GetAsync("http://bogusuri.no");

                //Assert
                Assert.Fail();
            }

            [TestMethod]
            public async Task AcceptsEmptyResponse()
            {
                //Arrange
                var client = GetClientWithResponseValidator(new FakeHttpClientHandlerForNoContentResponse());
                await client.GetAsync("http://bogusuri.no");

                //Act

                //Assert
            }
        }

    }
}