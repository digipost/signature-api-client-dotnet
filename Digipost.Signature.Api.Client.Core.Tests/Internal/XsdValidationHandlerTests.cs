using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public class XsdValidationHandlerTests
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

            private static HttpClient GetClient(DelegatingHandler lastHandler)
            {
                var client = HttpClientFactory.Create(
                    new XsdValidationHandler(),
                    lastHandler
                    );
                return client;
            }

        [TestClass]
        public class Request : XsdValidationHandlerTests
        {
            [TestMethod]
            public async Task AcceptsValidXml()
            {
                //Arrange
                var client = GetClient(new FakeHttpClientHandlerForDirectCreateResponse());
                HttpContent invalidRequestContent = new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetDirectSignatureJobRequestBody()).GetContent();

                //Act
                await client.SendAsync(GetHttpRequestMessage(invalidRequestContent));

                //Assert
            }

            [TestMethod]
            public async Task AcceptsGetRequest()
            {
                //Arrange
                var client = GetClient(new FakeHttpClientHandlerGetStatusResponse());

                //Act
                await client.GetAsync("http://bogusurl.no");

                //Assert
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidXmlException))]
            public async Task ThrowsExceptionOnInvalidXml()
            {
                //Arrange
                var client = GetClient(new FakeHttpClientHandlerForDirectCreateResponse());
                var invalidRequestContent = new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetDirectSignatureJobRequestBodyInvalid()).GetContent();

                //Act
                await client.SendAsync(GetHttpRequestMessage(invalidRequestContent));

                //Assert
                Assert.Fail();
            }

        }

        [TestClass]
        public class Response : XsdValidationHandlerTests
        {
            [TestMethod]
            public async Task AcceptsValidXml()
            {
                //Arrange
                var client = GetClient(new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetStatusResponse()));
                await client.GetAsync("http://bogusuri.no");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidXmlException))]
            public async Task ThrowsExceptionOnInvalidXml()
            {
                //Arrange
                var client = GetClient(new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetStatusResponseInvalid()));

                //Act
                await client.GetAsync("http://bogusuri.no");

                //Assert
                Assert.Fail();
            }

            [TestMethod]
            public async Task AcceptsEmptyResponse()
            {
                //Arrange
                var client = GetClient(new FakeHttpClientHandlerForNoContentResponse());
                await client.GetAsync("http://bogusuri.no");

                //Act

                //Assert
            }
        }
    }
}