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
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Internal
{
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

        public class SendAsync : XsdRequestValidationHandlerTests
        {
            private class FakeJob : IRequestContent
            {
            }

            [Fact]
            public async Task Accepts_request_with_no_body()
            {
                //Arrange
                var client = GetClientWithRequestValidator(new FakeHttpClientForDataResponse());

                //Act
                await client.GetAsync("http://bogusurl.no").ConfigureAwait(false);

                //Assert
            }

            [Fact]
            public async Task Accepts_request_with_xml_in_multipart()
            {
                //Arrange
                var client = GetClientWithRequestValidator(new FakeHttpClientForDataResponse());
                var validRequestContent = new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetDirectSignatureJobRequestBody()).GetContent();

                //Act
                await client.SendAsync(GetHttpRequestMessage(validRequestContent)).ConfigureAwait(false);

                //Assert
            }

            [Fact]
            public async Task Throws_exception_on_invalid_manifest_in_attachment()
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
                await Assert.ThrowsAsync<InvalidXmlException>(async () => await client.SendAsync(GetHttpRequestMessage(createAction.Content())).ConfigureAwait(false)).ConfigureAwait(false);

                //Assert
            }

            [Fact]
            public async Task Throws_exception_on_request_with_invalid_xml_in_multipart_body()
            {
                //Arrange
                var client = GetClientWithRequestValidator(new FakeHttpClientForDataResponse());
                var invalidRequestContent = new FakeHttpClientHandlerForMultipartXml(ContentUtility.GetDirectSignatureJobRequestBodyInvalid()).GetContent();

                //Act
                await Assert.ThrowsAsync<InvalidXmlException>(async () => await client.SendAsync(GetHttpRequestMessage(invalidRequestContent)).ConfigureAwait(false)).ConfigureAwait(false);
            }
        }
    }
}
