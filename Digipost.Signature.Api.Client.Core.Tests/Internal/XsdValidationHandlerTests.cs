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
        [TestClass]
        public class Request : XsdValidationHandlerTests
        {
            [TestMethod]
            public async Task AcceptsValidXml()
            {
                //Arrange
                var client = GetClient(new FakeHttpClientHandlerForDirectCreateResponse());
                var directSignatureJobRequestBody = ContentUtility.GetDirectSignatureJobRequestBody();
                var multipartFormDataContent = MultipartFormDataContent(directSignatureJobRequestBody);

                //Act
                await client.SendAsync(GetHttpRequestMessage(multipartFormDataContent));

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

            [ExpectedException(typeof (InvalidXmlException))]
            [TestMethod]
            public async Task ThrowsExceptionOnInvalidXml()
            {
                //Arrange
                var client = GetClient(new FakeHttpClientHandlerForDirectCreateResponse());
                var directSignatureJobRequestBody = ContentUtility.GetDirectSignatureJobRequestBodyInvalid();
                var multipartFormDataContent = MultipartFormDataContent(directSignatureJobRequestBody);

                //Act
                await client.SendAsync(GetHttpRequestMessage(multipartFormDataContent));

                //Assert
                Assert.Fail();
            }

            private static HttpRequestMessage GetHttpRequestMessage(MultipartFormDataContent multipartFormDataContent)
            {
                return new HttpRequestMessage
                {
                    Content = multipartFormDataContent,
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

            private static MultipartFormDataContent MultipartFormDataContent(string content)
            {
                var boundary = Guid.NewGuid().ToString();
                var multipartFormDataContent = new MultipartFormDataContent(boundary);

                var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/mixed");
                mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
                multipartFormDataContent.Headers.ContentType = mediaTypeHeaderValue;

                var directJobContent = new StringContent(content);
                directJobContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                directJobContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                multipartFormDataContent.Add(directJobContent);
                return multipartFormDataContent;
            }
        }

        [TestClass]
        public class Response : XsdValidationHandlerTests
        {
            [TestMethod]
            public void AcceptsValidXml()
            {
                //Arrange

                //Act

                //Assert
                Assert.Fail();
            }

            [TestMethod]
            public void ThrowsExceptionOnInvalidXml()
            {
                //Arrange

                //Act

                //Assert
                Assert.Fail();
            }
        }
    }
}