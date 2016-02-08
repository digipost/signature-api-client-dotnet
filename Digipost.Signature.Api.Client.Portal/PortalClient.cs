
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Portal.Internal;

namespace Digipost.Signature.Api.Client.Portal
{
    public class PortalClient : BaseClient
    {
        private static readonly Uri PortalJobSubPath = new Uri("/api/portal/signature-jobs", UriKind.Relative);
        
        public PortalClient(ClientConfiguration clientConfiguration) 
            : base(clientConfiguration)
        {
        }

        public async Task<PortalJobResponse> Create(PortalJob portalJob)
        {
            var documentBundle = AsiceGenerator.CreateAsice(ClientConfiguration.Sender, portalJob.Document,portalJob.Signers, ClientConfiguration.Certificate);
            var portalCreateAction = new PortalCreateAction(ClientConfiguration.Sender, portalJob, documentBundle);
            var requestResult = await HttpClient.PostAsync(PortalJobSubPath, portalCreateAction.Content());

            var strang = await requestResult.Content.ReadAsStringAsync();
            return PortalCreateAction.DeserializeFunc(strang);
        }

        public PortalJobStatusChanged GetStatusChange()
        {
            throw new NotImplementedException();
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
