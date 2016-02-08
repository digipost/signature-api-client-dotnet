using System;
using System.Net.Http;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Portal.Internal
{
    internal class PortalCreateAction : AbstractCreateAction
    {
        public static readonly Func<IRequestContent, Sender, string> SerializeFunc = (content, sender) => SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject((PortalJob)content, sender));

        public static readonly Func<string, PortalJobResponse> DeserializeFunc = content => DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<DTOPortalsignaturejobresponse>(content));

        public PortalCreateAction(Sender sender, PortalJob portalJob, DocumentBundle documentBundle) : base(sender, portalJob, documentBundle, SerializeFunc)
        {
        }
    }
}
