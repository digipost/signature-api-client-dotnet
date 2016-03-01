﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Internal;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using log4net;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectClient : BaseClient
    {
        private readonly Uri _subPath;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DirectClient(ClientConfiguration clientConfiguration)
            : base(clientConfiguration)
        {
            _subPath = new Uri($"/api/{clientConfiguration.Sender.OrganizationNumber}/direct/signature-jobs", UriKind.Relative);
            Log.Info($"Creating DirectClient, endpoint {new Uri(clientConfiguration.Environment.Url, _subPath)}");
        }

        public async Task<DirectJobResponse> Create(DirectJob directJob)
        {
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, directJob.Document, directJob.Signer, ClientConfiguration.Certificate);
            var createAction = new DirectCreateAction(directJob, documentBundle);

            return await RequestHelper.Create(_subPath, createAction.Content(), DirectCreateAction.DeserializeFunc);
        }

        public async Task<DirectJobStatusResponse> GetStatus(StatusReference statusReference)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = statusReference.Url,
                Method = HttpMethod.Get
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var requestResult = await HttpClient.SendAsync(request);
            var requestContent = requestResult.Content.ReadAsStringAsync().Result;
            
            switch (requestResult.StatusCode)
            {
                case HttpStatusCode.OK:
                    return DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<directsignaturejobstatusresponse>(requestContent));
                default:
                    throw RequestHelper.HandleGeneralException(requestContent, requestResult.StatusCode);
            }
        }

        public async Task<Stream> GetXades(XadesReference xadesReference)
        {
            return await RequestHelper.GetStream(xadesReference.Url);
        }

        public async Task<Stream> GetPades(PadesReference padesReference)
        {
            return await RequestHelper.GetStream(padesReference.Url);
        }

        public async Task Confirm(ConfirmationReference confirmationReference)
        {
            await RequestHelper.Confirm(confirmationReference);
        }

        internal async Task AutoSign(long jobId)
        {
            var url = new Uri($"/web/signature-jobs/{jobId}/devmodesign", UriKind.Relative);
            await HttpClient.PostAsync(url, null);
        }
    }
}