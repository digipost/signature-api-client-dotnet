using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Portal.Internal;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
using DataTransferObjectConverter = Digipost.Signature.Api.Client.Portal.DataTransferObjects.DataTransferObjectConverter;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalClient : BaseClient
    {
        private readonly Uri _subPath;

        public PortalClient(ClientConfiguration clientConfiguration) 
            : base(clientConfiguration)
        {
            _subPath = new Uri(string.Format("/api/{0}/portal/signature-jobs", clientConfiguration.Sender.OrganizationNumber), UriKind.Relative);
        }

        public async Task<PortalJobResponse> Create(PortalJob portalJob)
        {
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, portalJob.Document,portalJob.Signers, ClientConfiguration.Certificate);
            var portalCreateAction = new PortalCreateAction(portalJob, documentBundle);
            var requestResult = await HttpClient.PostAsync(_subPath, portalCreateAction.Content());

            return PortalCreateAction.DeserializeFunc(await requestResult.Content.ReadAsStringAsync());
        }

        public async Task<PortalJobStatusChangeResponse> GetStatusChange()
        {
            var requestResult = await HttpClient.GetAsync(_subPath);
            var deserialized = SerializeUtility.Deserialize<portalsignaturejobstatuschangeresponse>(await requestResult.Content.ReadAsStringAsync());

            return DataTransferObjectConverter.FromDataTransferObject(deserialized);
        }

        public async Task<Stream> GetXades(XadesReference xadesReference)
        {
            throw new NotImplementedException();

            //return await HttpClient.GetStreamAsync(xadesReference.XadesUri);
        }

        public async Task<Stream> GetPades(PadesReference padesReference)
        {
            throw new NotImplementedException();

            //return await HttpClient.GetStreamAsync(padesReference.PadesUri);
        }

        public async Task<HttpResponseMessage> Confirm(ConfirmationReference confirmationReference)
        {
            throw new NotImplementedException();
            //return await HttpClient.PostAsync(confirmationReference.ConfirmationUri, content: null);
        }
    }
}
