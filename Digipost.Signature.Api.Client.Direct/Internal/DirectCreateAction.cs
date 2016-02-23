using System;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Direct.Internal
{
    internal class DirectCreateAction : CreateAction
    {
        public static readonly Func<IRequestContent, string> SerializeFunc = content => SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject((DirectJob) content));
        public static readonly Func<string, DirectJobResponse> DeserializeFunc = content => DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<directsignaturejobresponse>(content));

        public DirectCreateAction(DirectJob directJob, DocumentBundle documentBundle)
            : base(directJob, documentBundle, SerializeFunc)
        {
        }
    }
}