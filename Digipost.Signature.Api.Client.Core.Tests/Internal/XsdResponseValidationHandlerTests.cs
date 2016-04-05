using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Direct.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Internal
{
    [TestClass]
    public class XsdResponseValidationHandlerTests
    {
        private static HttpClient GetClientWithResponseValidator(DelegatingHandler lastHandler)
        {
            var client = HttpClientFactory.Create(
                new XsdResponseValidationHandler(),
                lastHandler
                );
            return client;
        }

        [TestClass]
        public class SendAsync : XsdResponseValidationHandlerTests
        {
            [TestMethod]
            [ExpectedException(typeof (InvalidXmlException))]
            public async Task ThrowsExceptionOnResponseWithInvalidXmlInContent()
            {
                var client = GetClientWithResponseValidator(new FakeHttpClientHandlerForStatusResponseInvalid());
                await client.GetAsync(new Uri("http://bogusuri.no"));

                //Act

                //Assert
                Assert.Fail();
            }

            [TestMethod]
            public async Task AcceptsResponseWithXmlInContent()
            {
                //Arrange
                var client = GetClientWithResponseValidator(new FakeHttpClientHandlerForStatusResponse());

                //Act
                await client.GetAsync(new Uri("http://bogusuri.no"));

                //Assert
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

            [TestMethod]
            public async Task AcceptsXadesResponse()
            {
                //Arrange
                var uriPartToEnableIgnoringOfXadesXmlValidation = "xades";
                var client = GetClientWithResponseValidator(new FakeHttpClientHandlerForXadesResponse());

                //Act
                await client.GetAsync($"http://bogusuri.no/{uriPartToEnableIgnoringOfXadesXmlValidation}");

                //Assert
            }
        }
    }
}