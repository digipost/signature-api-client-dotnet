using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Digipost.Signature.Api.Client.Core.Asice;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal abstract class CreateAction : SignatureAction
    {
        private readonly DocumentBundle _documentBundle;

        public MultipartFormDataContent MultipartFormDataContent { get; internal set; }

        protected CreateAction(IRequestContent job, DocumentBundle documentBundle, Func<IRequestContent, string> serializeFunc) : base(job, serializeFunc)
        {
            _documentBundle = documentBundle;
        }

        internal override HttpContent Content()
        {
            var boundary = Guid.NewGuid().ToString();

            MultipartFormDataContent = new MultipartFormDataContent(boundary);

            var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/mixed");
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            MultipartFormDataContent.Headers.ContentType = mediaTypeHeaderValue;

            MultipartFormDataContent.Add(BodyContent());
            MultipartFormDataContent.Add(AddDocumentBundle());

            return MultipartFormDataContent;
        }

        private StringContent BodyContent()
        {
            var directJobContent = new StringContent(SerializedBody);
            directJobContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            directJobContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

            return directJobContent;
        }

        private ByteArrayContent AddDocumentBundle()
        {
            var documentBundleContent = new ByteArrayContent(_documentBundle.BundleBytes);
            documentBundleContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            documentBundleContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

            return documentBundleContent;
        }
    }
}
