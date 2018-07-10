using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    internal class FakeHttpClientHandlerForMultipartXml : FakeHttpClientHandlerResponse
    {
        public FakeHttpClientHandlerForMultipartXml(string content)
        {
            Content = content;
        }

        public string Content { get; set; }

        public override HttpContent GetContent()
        {
            return MultipartFormDataContent(Content);
        }

        private static MultipartFormDataContent MultipartFormDataContent(string content)
        {
            var boundary = Guid.NewGuid().ToString();
            var multipartFormDataContent = new MultipartFormDataContent(boundary);

            var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/mixed");
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            multipartFormDataContent.Headers.ContentType = mediaTypeHeaderValue;

            var stringContent = new StringContent(content);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

            multipartFormDataContent.Add(stringContent);
            return multipartFormDataContent;
        }
    }
}