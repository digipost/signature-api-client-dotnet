using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Direct.Internal
{
    internal class CreateAction : DigipostAction
    {
        public static readonly Func<IRequestContent, Sender, string> SerializeFunc = (content,sender) => SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject((DirectJob) content, sender));
        public static readonly Func<string, DirectJobResponse> DeserializeFunc = content => DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<DirectJobResponseDataTransferObject>(content));

        private readonly DocumentBundle _documentBundle;

        public MultipartFormDataContent MultipartFormDataContent { get; internal set; }

        public CreateAction(Sender sender, DirectJob directJob, DocumentBundle documentBundle) : base(directJob, sender, SerializeFunc)
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
