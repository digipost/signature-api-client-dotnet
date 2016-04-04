using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Internal
{
    [TestClass]
    public class XsdRequestValidationHandlerTests
    {
        private static HttpRequestMessage GetHttpRequestMessage(HttpContent httpContent)
        {
            return new HttpRequestMessage
            {
                Content = httpContent,
                RequestUri = new Uri("http://bogusurl.no"),
                Method = HttpMethod.Post
            };
        }

        private static HttpClient GetClientWithRequestValidator(DelegatingHandler lastHandler)
        {
            var client = HttpClientFactory.Create(
                new XsdRequestValidationHandler(),
                lastHandler
                );
            return client;
        }

        [TestClass]
        public class SendAsync : XsdRequestValidationHandlerTests
        {
            [TestMethod]
            public async Task AcceptsValidXml()
            {
                //Arrange
                var client = GetClientWithRequestValidator(new FakeHttpClientHandlerForDirectCreateResponse());
                var invalidRequestContent = new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetDirectSignatureJobRequestBody()).GetContent();

                //Act
                await client.SendAsync(GetHttpRequestMessage(invalidRequestContent));

                //Assert
            }

            [TestMethod]
            public async Task AcceptsGetRequest()
            {
                //Arrange
                var client = GetClientWithRequestValidator(new FakeHttpClientHandlerGetStatusResponse());

                //Act
                await client.GetAsync("http://bogusurl.no");

                //Assert
            }

            [TestMethod]
            [ExpectedException(typeof (InvalidXmlException))]
            public async Task ThrowsExceptionOnInvalidXml()
            {
                //Arrange
                var client = GetClientWithRequestValidator(new FakeHttpClientHandlerForDirectCreateResponse());
                var invalidRequestContent = new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetDirectSignatureJobRequestBodyInvalid()).GetContent();

                //Act
                await client.SendAsync(GetHttpRequestMessage(invalidRequestContent));

                //Assert
                Assert.Fail();
            }
        }
    }
}