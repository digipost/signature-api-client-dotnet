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
        private string _serializedBody;

        public MultipartFormDataContent MultipartFormDataContent { get; internal set; }

        public CreateAction(DirectJob directJob, DocumentBundle documentBundle, X509Certificate2 businessCertificate, Uri signatureServiceRoot) : base(directJob, businessCertificate, signatureServiceRoot)
        {
            _documentBundle = documentBundle;
        }

        protected override HttpContent Content()
        {
            var boundary = Guid.NewGuid().ToString();

            MultipartFormDataContent = new MultipartFormDataContent(boundary);

            var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/mixed");
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            MultipartFormDataContent.Headers.ContentType = mediaTypeHeaderValue;

            AddBodyToContent();
            AddDocumentBundle();
            
            return MultipartFormDataContent; ;
        }

        private void AddBodyToContent()
        {
            var directJobContent = new StringContent(Serialize());
            directJobContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            directJobContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "\"message\""
            };

            MultipartFormDataContent.Add(directJobContent);
        }

        private void AddDocumentBundle()
        {
            var documentBundleContent = new ByteArrayContent(_documentBundle.BundleBytes);
            documentBundleContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            documentBundleContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "documentBundle"
            };
            MultipartFormDataContent.Add(documentBundleContent);
        }

        protected override string Serialize()
        {
            if (_serializedBody == null)
            {
                var directJobDataTranferObject = DataTransferObjectConverter.ToDataTransferObject((DirectJob)RequestContent);
                return _serializedBody = SerializeUtility.Serialize(directJobDataTranferObject);
            }

            return _serializedBody;
        }
    }
}
