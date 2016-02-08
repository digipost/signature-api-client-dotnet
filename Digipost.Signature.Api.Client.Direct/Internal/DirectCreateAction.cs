using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Direct.Internal
{
    internal class DirectCreateAction : CreateAction
    {
        public static readonly Func<IRequestContent, Sender, string> SerializeFunc = (content,sender) => SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject((DirectJob) content, sender));
        public static readonly Func<string, DirectJobResponse> DeserializeFunc = content => DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<DirectJobResponseDataTransferObject>(content));

        public DirectCreateAction(Sender sender, DirectJob directJob, DocumentBundle documentBundle) : base(sender, directJob, documentBundle, SerializeFunc)
        {
        }
    }
}
