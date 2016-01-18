using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Direct.Internal
{
    internal class CreateAction : DigipostAction
    {
        private readonly DocumentBundle _documentBundle;
        private MultipartFormDataContent _multipartFormDataContent;

        public CreateAction(DirectJob directJob, DocumentBundle documentBundle, X509Certificate2 businessCertificate, Uri signatureServiceRoot) : base(directJob, businessCertificate, signatureServiceRoot)
        {
            _documentBundle = documentBundle;
        }

        protected override HttpContent Content()
        {
            var message = RequestContent as DirectJob;
            var boundary = Guid.NewGuid().ToString();

            _multipartFormDataContent = new MultipartFormDataContent(boundary);

            var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/mixed");
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            _multipartFormDataContent.Headers.ContentType = mediaTypeHeaderValue;

            AddBodyToContent(message);
            AddDocumentBundle();

            return _multipartFormDataContent; ;
        }

        private void AddBodyToContent(DirectJob directJob)
        {
            var directJobDataTransferObject = DataTransferObjectConverter.ToDataTransferObject(directJob);
            var directJobSerialized = SerializeUtility.Serialize(directJobDataTransferObject);

            var directJobContent = new StringContent(directJobSerialized);
            directJobContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            directJobContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "\"message\""
            };

            _multipartFormDataContent.Add(directJobContent);
        }

        private void AddDocumentBundle()
        {
            var documentBundleContent = new ByteArrayContent(_documentBundle.BundleBytes);
            documentBundleContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            documentBundleContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "documentBundle"
            };
            _multipartFormDataContent.Add(documentBundleContent);
        }

        protected override string Serialize()
        {
            //TODO: This method does duplicate work, just for convenience of Xml-Validation. Fix!
            var directJobDataTranferObject = DataTransferObjectConverter.ToDataTransferObject((DirectJob)RequestContent );
            return SerializeUtility.Serialize(directJobDataTranferObject);
        }
    }
}
