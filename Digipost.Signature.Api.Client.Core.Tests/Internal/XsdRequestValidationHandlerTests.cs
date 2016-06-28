using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Resources.Xml;
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
            public async Task AcceptsRequestWithXmlInMultipart()
            {
                //Arrange
                var client = GetClientWithRequestValidator(new FakeHttpClientForDataResponse());
                var validRequestContent = new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetDirectSignatureJobRequestBody()).GetContent();

                //Act
                await client.SendAsync(GetHttpRequestMessage(validRequestContent));

                //Assert
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidXmlException))]
            public async Task ThrowsExceptionOnRequestWithInvalidXmlInMultipartBody()
            {
                //Arrange
                var client = GetClientWithRequestValidator(new FakeHttpClientForDataResponse());
                var invalidRequestContent = new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetDirectSignatureJobRequestBodyInvalid()).GetContent();

                //Act
                await client.SendAsync(GetHttpRequestMessage(invalidRequestContent));

                //Assert
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidXmlException))]
            public async Task ThrowsExceptionOnInvalidManifestInAttachment()
            {
                //Arrange
                var client = GetClientWithRequestValidator(new FakeHttpClientForDataResponse());

                var serializedfunc = new Func<IRequestContent, string>(p => ContentUtility.GetDirectSignatureJobRequestBody());

                var manifestBytes = Encoding.UTF8.GetBytes(XmlResource.Request.GetPortalManifest().OuterXml);
                var asiceArchive = new AsiceArchive(new List<AsiceAttachableProcessor>());
                asiceArchive.AddAttachable("manifest.xml", manifestBytes);
                var documentBundle = new DocumentBundle(asiceArchive.GetBytes());

                var createAction = new CreateAction(new FakeJob(), documentBundle, serializedfunc);

                //Act
                await client.SendAsync(GetHttpRequestMessage(createAction.Content()));

                //Assert
            }

            [TestMethod]
            public async Task AcceptsRequestWithNoBody()
            {
                //Arrange
                var client = GetClientWithRequestValidator(new FakeHttpClientForDataResponse());

                //Act
                await client.GetAsync("http://bogusurl.no");

                //Assert
            }

            private class FakeJob : IRequestContent
            {
            }
        }
    }
}